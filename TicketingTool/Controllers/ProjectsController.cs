using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TicketingTool.Data;
using TicketingTool.Models;
using TicketingTool.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class ProjectsController : Controller
{
    private readonly ApplicationDBContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectsController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.Identity.Name; 

        // Fetch projects assigned to the user or visible to an Admin
        var isAdmin = await _context.ProjectUserRole
            .AnyAsync(pur => pur.UserId == userId && pur.RoleId == "ADMIN");

        var projects = isAdmin
            ? await _context.Project.Include(p => p.UserRoles).ToListAsync() // Admin sees all projects
            : await _context.Project
                .Include(p => p.UserRoles)
                .Where(p => p.UserRoles.Any(ur => ur.UserId == userId))
                .ToListAsync(); // Other users see only their assigned projects

        return View(projects);
    }


    public async Task<IActionResult> Details(int id)
    {
        var userId = User.Identity.Name; // Get the logged-in user's ID

        var project = await _context.Project
            .Include(p => p.Components)
            .Include(p => p.UserRoles)
            .ThenInclude(pur => pur.UserNameRef) // Include user details for assignees
            .FirstOrDefaultAsync(p => p.ID == id);

        if (project == null)
        {
            return NotFound();
        }

        // Check if the user is assigned to the project
        var userRole = project.UserRoles.FirstOrDefault(ur => ur.UserId == userId);

        // Allow only Admins or Managers to see the details
        if (userRole == null || (userRole.RoleId != "MANAGER" && userRole.RoleId != "ADMIN"))
        {
            return Forbid();
        }

        return View(project);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComponent(int ProjectId, string componentName)
    {
        var userId = User.Identity.Name;

        var project = await _context.Project
            .Include(p => p.UserRoles)
            .FirstOrDefaultAsync(p => p.ID == ProjectId);

        var component = new Component
        {
            ComponentName = componentName,
            ProjectID = ProjectId
        };

        await _context.Components.AddAsync(component);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = ProjectId });
    }
}
