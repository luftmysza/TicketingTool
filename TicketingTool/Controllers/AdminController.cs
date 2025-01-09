using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TicketingTool.Models;
using TicketingTool.Areas.Identity.Data;
using TicketingTool.Data;
using Microsoft.AspNetCore.Authorization;

namespace TicketingTool.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.Projects = _context.Project.ToList();
            ViewBag.Users = _userManager.Users.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateProject(string projectName, string projectKey)
        {
            if (!string.IsNullOrWhiteSpace(projectName) && !string.IsNullOrWhiteSpace(projectKey))
            {
                var project = new Project
                {
                    ProjectName = projectName,
                    ProjectKey = projectKey,
                    Counter = 0
                };
                _context.Project.Add(project);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Project created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Project name and key are required.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserToProject(string UserName, int projectId, string role)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == UserName);
            var project = _context.Project.FirstOrDefault(p => p.ID == projectId);

            if (user != null && project != null && !string.IsNullOrWhiteSpace(role))
            {
                var rule = new ProjectUserRole { UserId = UserName, ProjectId = projectId, RoleId = role };
                await _context.ProjectUserRole.AddAsync(rule);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"User {user.UserName} assigned to project {project.ProjectName} as {role}.";
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid user, project, or role.";
            }

            return RedirectToAction("Index");
        }
    }
}
