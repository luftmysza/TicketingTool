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
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<Ticket> applicationDBContext =  
                await _context.Ticket
                    .Include(t => t.ProjectRef)
                    .Include(t => t.StatusRef)
                    .Include(t => t.ComponentRef)
                    .Include(t => t.AssigneeRef)
                    .Include(t => t.CreatorRef)
                    .ToListAsync();

            return View(applicationDBContext);
        }

        // GET: Tickets/Details/5
        [HttpGet("Details/{issueKey?}")]
        public async Task<IActionResult> Details(string issueKey)
        {
            if (issueKey == null)
            {
                return NotFound();
            }

            Ticket? ticket = await _context.Ticket
                .Include(t => t.ProjectRef)
                .Include(t => t.StatusRef)
                .Include(t => t.ComponentRef)
                .Include(t => t.AssigneeRef)
                .Include(t => t.CreatorRef)
                .FirstOrDefaultAsync(x => x.IssueKey == issueKey);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["Component"] = new SelectList(_context.Set<Component>(), "ID", "ComponentName");
            ViewData["Project"] = new SelectList(_context.Set<Project>(), "ID", "ProjectName");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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

            var ticket = 
                await _context.Ticket

                    .Include(t => t.StatusRef)
                    .Include(t => t.ComponentRef)
                    .Include(t => t.AssigneeRef)
                    .Include(t => t.CreatorRef)
                    .FirstOrDefaultAsync(x => x.IssueKey == issueKey);

            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["Component"] = new SelectList(_context.Set<Component>(), "ID", "ComponentName", ticket.ComponentID);
            ViewData["Project"] = new SelectList(_context.Set<Project>(), "ID", "ProjectName", ticket.ProjectID);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,IssueKey,ProjectID,ComponentID,Title,Description,CreatorID,Status,AssigneeID,CreatedDate,LastUpdatedDate,ResolvedDate")] Ticket ticket)
        {
            if (id != ticket.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Component"] = new SelectList(_context.Set<Component>(), "ID", "ComponentName", ticket.ComponentID);
            ViewData["Project"] = new SelectList(_context.Set<Project>(), "ID", "ProjectName", ticket.ProjectID);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.ComponentRef)
                .Include(t => t.ProjectRef)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.ID == id);
        }
    }
}
