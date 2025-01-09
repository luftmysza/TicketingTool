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

        // GET: Tickets
        [HttpGet("Browse/Index")]
        public async Task<IActionResult> Index()
        {
            List<Ticket> tickets = await GetTicketsAsync(false);

            return View(tickets);
        }

        // GET: Tickets/Details/5
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

        // GET: Tickets/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["Component"] = new SelectList(_context.Set<Component>(), "ID", "ComponentName");
            ViewData["Project"] = new SelectList(_context.Set<Project>(), "ID", "ProjectName");
            return View();
        }

        // POST: Tickets/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,IssueKey,ProjectID,ComponentID,Title,Description,CreatorID,Status,AssigneeID,CreatedDate,LastUpdatedDate,ResolvedDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Component"] = new SelectList(_context.Set<Component>(), "ID", "ComponentName", ticket.ComponentID);
            ViewData["Project"] = new SelectList(_context.Set<Project>(), "ID", "ProjectName", ticket.ProjectID);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
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

            ViewData["Project"] = new SelectList(_context.Set<Project>(), "ID", "ProjectName", ticket.ProjectID);

            return View(ticket);
        }

        // POST: Tickets/Edit/5
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
                //return View(ticketNew);
                return RedirectToAction(nameof(Details), "Browse", new { issueKey = ticketNew.IssueKey });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["Component"] = new SelectList(
                _context.Set<Component>()
                    .Where(c => c.ProjectID == ticketNew.ProjectID),
                "ID",
                "ComponentName",
                ticketNew.ComponentID
            );
            ViewData["Project"] = new SelectList(_context.Set<Project>(), "ID", "ProjectName", ticketNew.ProjectID);
            return View(ticketNew);
        }

        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(string IssueKey, string Content)
        {
            if (string.IsNullOrEmpty(Content))
            {
                return RedirectToAction("Details", new { issueKey = IssueKey });
            }

            // Get the user making the comment
            var userName = User.Identity.Name;
            var user = _context.Users.SingleOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                return Unauthorized();
            }

            // Ensure the referenced Ticket exists by IssueKey
            var ticket = await GetTicketAsync(IssueKey, false);
            if (ticket == null)
            {
                return NotFound();
            }

            // Create and add the comment
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

        public IActionResult DeleteComment(string IssueKey)
        {
            var comment = _context.Comments.Find(IssueKey);
            if (comment == null)
            {
                return NotFound();
            }

            var issueKey = comment.IssueKey;
            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return RedirectToAction("Details", new { issueKey = issueKey });
        }

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
    