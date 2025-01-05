using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingTool.Models;
using Microsoft.AspNetCore.Identity;

namespace TicketingTool.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ICollection<Project> Projects { get; set; } = new List<Project>();

    public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();

    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
}

