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

        var isAdmin = await _context.ProjectUserRole
            .AnyAsync(pur => pur.UserId == userId && pur.RoleId == "ADMIN");

        var projects = isAdmin
            ? await _context.Project.Include(p => p.UserRoles).ToListAsync()
            : await _context.Project
                .Include(p => p.UserRoles)
                .Where(p => p.UserRoles.Any(ur => ur.UserId == userId))
                .ToListAsync(); 

        return View(projects);
    }


    public async Task<IActionResult> Details(int id)
    {
        var userId = User.Identity.Name; 

        var project = await _context.Project
            .Include(p => p.Components)
            .Include(p => p.UserRoles)
            .ThenInclude(pur => pur.UserNameRef) 
            .FirstOrDefaultAsync(p => p.ID == id);

        if (project == null)
        {
            return NotFound();
        }

        var userRole = project.UserRoles.FirstOrDefault(ur => ur.UserId == userId);

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
        if (componentName.Length > 30)
        {
            TempData["ErrorMessage"] = "Component name must be up to 30 characters.";
            return RedirectToAction(nameof(Details), new { id = ProjectId });
        }
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
