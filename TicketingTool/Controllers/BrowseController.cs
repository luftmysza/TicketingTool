using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketingTool.Data;
using TicketingTool.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TicketingTool.Areas.Identity.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Channels;
using System.Reflection;
using System.Net.Sockets;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;



namespace TicketingTool.Controllers
{
    [Authorize]
    [Route("browse")]
    public class BrowseController : Controller
    {
        private readonly ApplicationDBContext _context;


        public BrowseController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet("Browse/Index")]
        public async Task<IActionResult> Index()
        {
            string currentUser = User.Identity.Name;

            List<int> userProjectIds = await _context.ProjectUserRole
                    .Where(pur => pur.UserId == currentUser)
                    .Select(pur => pur.ProjectId)
                    .Distinct()
                    .ToListAsync();

            List<Ticket> tickets = await _context.Ticket
                .Where(t => userProjectIds.Contains(t.ProjectID))
                .ToListAsync();


            return View(tickets);
        }

        [HttpGet("Browse/MyTickets")]
        public async Task<IActionResult> MyTickets()
        {
            List<Ticket> tickets = await _context.Ticket.Where(x => x.AssigneeID == User.Identity.Name || x.CreatorID == User.Identity.Name).ToListAsync();

            return View(tickets);
        }

        [HttpGet("Details/{issueKey?}")]
        public async Task<IActionResult> Details(string issueKey)
        {
            if (issueKey == null)
            {
                return NotFound();
            }

            Ticket? ticket = await GetTicketAsync(issueKey, false);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        [HttpGet("Browse/CreateStep1")]

        public IActionResult CreateStep1()
        {
            var currentUser = User.Identity.Name;

            var projects = _context.Project
                .Where(p => _context.ProjectUserRole.Any(pur => pur.UserId == currentUser && pur.ProjectId == p.ID))
                .ToList();

            return View(projects);
        }

        [HttpGet("Browse/CreateStep2/")]
        public IActionResult CreateStep2(int id)
        {
            ViewData["Component"] = new SelectList(
               _context.Set<Component>()
                   .Where(c => c.ProjectID == id),
               "ID",
               "ComponentName",
            1
           );
            ViewBag.ProjectId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            try
            {
                string currentUser = User.Identity.Name;
                Models.Project project = await _context.Project.FirstOrDefaultAsync(p => p.ID == ticket.ProjectID);

                ticket.IssueKey = $"{project.ProjectKey}-{project.Counter+1}";
                ticket.CreatorID = User.Identity.Name;
                ticket.CreatedDate = DateTime.Now;
                ticket.LastUpdatedDate = DateTime.Now;
                ticket.StatusID = 1;
                project.Counter++;

                _context.Ticket.Add(ticket);
                _context.Project.Update(project);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Ticket created successfully!";
                return RedirectToAction("Details", "Browse", new {issueKey = ticket.IssueKey});
            }
            catch
            {
                return View("SomthingWentWrong");
            }
        }

        [HttpGet("Edit/{issueKey}")]
        public async Task<IActionResult> Edit(string issueKey)
        {
            if (issueKey == null)
            {
                return NotFound();
            }

            var ticket = await GetTicketAsync(issueKey, false);

            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["Component"] = new SelectList(
                _context.Set<Component>()
                .Where(c => c.ProjectID == ticket.ProjectID),
                "ID",
                "ComponentName",
                ticket.ComponentID
            );

            ViewData["Status"] = new SelectList(
                _context.Set<Status>(),
                "ID",
                "StatusName",
                ticket.StatusID
            );

            ViewData["Project"] = new SelectList(_context.Set<Models.Project>(), "ID", "ProjectName", ticket.ProjectID);

            return View(ticket);
        }

        [HttpPost("Edit/{issueKey}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string issueKey, Ticket? ticket)
        {
            Ticket? ticketOld = await GetTicketAsync(issueKey, true);
            Ticket? ticketNew = null;

            if (issueKey != ticket!.IssueKey || ticketOld is null)
            {
                return NotFound();
            }
            var assigneeExists = _context.Users.Any(u => u.UserName == ticket.AssigneeID); 
            if (!assigneeExists)
            {
                ModelState.AddModelError("AssigneeID", "The assignee does not exist.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();

                    ticketNew = await GetTicketAsync(ticket!.IssueKey, true);

                    await LogChangesAsync(ticketOld, ticketNew, User.Identity?.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Ticket.Exists(_context, ticketNew.IssueKey))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), "Browse", new { issueKey = ticketNew.IssueKey });
            }
            ViewData["Component"] = new SelectList(
                _context.Set<Component>()
                    .Where(c => c.ProjectID == ticket.ProjectID),
                "ID",
                "ComponentName",
                ticket.ComponentID
            );
            ViewData["Status"] = new SelectList(
               _context.Set<Status>(),
               "ID",
               "StatusName",
               ticket.StatusID
           );
            ViewData["Project"] = new SelectList(_context.Set<Models.Project>(), "ID", "ProjectName", ticket.ProjectID);
            return View(ticket);
        }

        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(string IssueKey, string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return RedirectToAction("Details", new { issueKey = IssueKey });
            }

            var userName = User.Identity.Name;
            var user = _context.Users.SingleOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return Unauthorized();
            }

            var ticket = await GetTicketAsync(IssueKey, false);
            if (ticket == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                IssueKey = IssueKey,
                Content = Content,
                AuthorUserName = user.UserName,
                AuthorRef = user,
                CreatedDate = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", new { issueKey = IssueKey });
        }

        //TODO
        //public IActionResult DeleteComment(string IssueKey)
        //{
        //    var comment = _context.Comments.Find(IssueKey);
        //    if (comment == null)
        //    {
        //        return NotFound();
        //    }

        //    var issueKey = comment.IssueKey;
        //    _context.Comments.Remove(comment);
        //    _context.SaveChanges();

        //    return RedirectToAction("Details", new { issueKey = issueKey });
        //}

        private async Task LogChangesAsync(Ticket ticketOld, Ticket ticketNew, string changedBy)
        {
            var fields = await TicketField.GetTicketFields(ticketOld);

            foreach (var field in fields)
            {
                try
                {
                    string? oldValue = null;
                    string? newValue = null;

                    if (field.FieldName.EndsWith("Ref"))
                    {
                        var navigationProperty = typeof(Ticket).GetProperty(field.FieldName);
                        if (navigationProperty == null) continue;

                        var oldNavObject = navigationProperty.GetValue(ticketOld);
                        var newNavObject = navigationProperty.GetValue(ticketNew);

                        if (oldNavObject != null && newNavObject != null)
                        {
                            var navProperties = oldNavObject.GetType().GetProperties();

                            foreach (var navProperty in navProperties)
                            {
                                if (navProperty.Name.EndsWith("ID")) continue; 

                                var oldNavValue = navProperty.GetValue(oldNavObject)?.ToString();
                                var newNavValue = navProperty.GetValue(newNavObject)?.ToString();

                                if (oldNavValue != newNavValue)
                                {
                                    var changes = new TicketChange
                                    {
                                        TicketID = ticketOld.ID,
                                        ChangedFieldId = field.ID,
                                        ChangedFieldName = $"{field.FieldName}.{navProperty.Name}",
                                        OldValue = oldNavValue,
                                        NewValue = newNavValue,
                                        ChangedBy = changedBy,
                                        ChangedAt = DateTime.Now
                                    };
                                    _context.TicketChange.Add(changes);
                                }
                            }
                        }
                    }
                    
                }
                catch
                {
                    continue;
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task<Ticket?> GetTicketAsync(string issueKey, bool asNoTracking)
        {
            Ticket? ticket = null;
            if (asNoTracking is true)
            {
                ticket = await _context.Ticket
                    .AsNoTracking()
                    .Include(t => t.ProjectRef)
                    .Include(t => t.StatusRef)
                    .Include(t => t.ComponentRef)
                    .Include(t => t.AssigneeRef)
                    .Include(t => t.CreatorRef)
                    .Include(t => t.Changes)
                    .Include(t => t.Comments)
                    .FirstOrDefaultAsync(t => t.IssueKey == issueKey);
            }
            else
            {
                ticket = await _context.Ticket
                    .Include(t => t.ProjectRef)
                    .Include(t => t.StatusRef)
                    .Include(t => t.ComponentRef)
                    .Include(t => t.AssigneeRef)
                    .Include(t => t.CreatorRef)
                    .Include(t => t.Changes)
                    .Include(t => t.Comments)
                    .FirstOrDefaultAsync(t => t.IssueKey == issueKey);
            }
            return ticket;
        }

        private async Task<List<Ticket>> GetTicketsAsync(bool asNoTracking)
        {
            List<Ticket> tickets = new List<Ticket>();
            if (asNoTracking is true)
            {
                tickets = await _context.Ticket
                    .AsNoTracking()
                    .Include(t => t.ProjectRef)
                    .Include(t => t.StatusRef)
                    .Include(t => t.ComponentRef)
                    .Include(t => t.AssigneeRef)
                    .Include(t => t.CreatorRef)
                    .Include(t => t.Changes)
                    .ToListAsync();
            }
            else
            {
                tickets = await _context.Ticket
                    .Include(t => t.ProjectRef)
                    .Include(t => t.StatusRef)
                    .Include(t => t.ComponentRef)
                    .Include(t => t.AssigneeRef)
                    .Include(t => t.CreatorRef)
                    .Include(t => t.Changes)
                    .ToListAsync();
            }
            return tickets;
        }
    }
}
    