using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TicketingTool.Models;
using TicketingTool.Areas.Identity.Data;
using TicketingTool.Data;
using Microsoft.AspNetCore.Authorization;
using System.Net.Sockets;

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
            string[] systemUser = { "X001", "TECH02", "TECH01" };
            ViewBag.Projects = _context.Project.ToList();
            ViewBag.Users = _userManager.Users
                .Where(u => !systemUser.Contains(u.UserName))
                .ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateProject(string projectName, string projectKey)
        {
            var projectExists = _context.Project.Any(p => p.ProjectKey == projectKey || p.ProjectName == projectName);
            if (projectExists)
            {
                ModelState.AddModelError("projectKey", "Such project already exists.");
                TempData["ErrorMessage"] = "Such project already exists.";
            }
            if (ModelState.IsValid)
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

                    var standardUsers = new List<ProjectUserRole>
                {
                    new ProjectUserRole
                    {
                        ProjectId = project.ID,
                        UserId = "X001",
                        RoleId = "ADMIN"
                    },
                    new ProjectUserRole
                    {
                        ProjectId = project.ID,
                        UserId = "TECH01",
                        RoleId = "TECH"
                    },
                    new ProjectUserRole
                    {
                        ProjectId = project.ID,
                        UserId = "TECH02",
                        RoleId = "TECH"
                    }
                };

                    _context.ProjectUserRole.AddRange(standardUsers);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Project created successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Project name and key are required.";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserToProject(string UserName, int projectId, string role)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == UserName);
            var project = _context.Project.FirstOrDefault(p => p.ID == projectId);

            if (user == null || project == null || string.IsNullOrWhiteSpace(role))
            {
                TempData["ErrorMessage"] = "Invalid user, project, or role.";
                return RedirectToAction("Index");
            }

            var rulePartialExists = _context.ProjectUserRole.Any(pur => pur.ProjectId == project.ID && pur.UserId == user.UserName);
            if (rulePartialExists)
            {
                var ruleExists = _context.ProjectUserRole.Any(pur => pur.ProjectId == project.ID && pur.UserId == user.UserName && pur.RoleId == role);
                if (ruleExists)
                {
                    ModelState.AddModelError("projectKey", "This user is already assigned to the project with this role.");
                    TempData["ErrorMessage"] = "This user is already assigned to the project with this role.";
                    return RedirectToAction("Index"); 
                }

                var rule = _context.ProjectUserRole.FirstOrDefault(pur => pur.ProjectId == project.ID && pur.UserId == user.UserName);
                if (rule != null)
                {
                    rule.RoleId = role;
                    _context.ProjectUserRole.Update(rule);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"User {user.UserName} updated to role {role} in project {project.ProjectName}.";
                    return RedirectToAction("Index"); 
                }
            }

            var newRule = new ProjectUserRole { UserId = UserName, ProjectId = projectId, RoleId = role };
            await _context.ProjectUserRole.AddAsync(newRule);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"User {user.UserName} assigned to project {project.ProjectName} as {role}.";

            return RedirectToAction("Index");
        }

    }
}
