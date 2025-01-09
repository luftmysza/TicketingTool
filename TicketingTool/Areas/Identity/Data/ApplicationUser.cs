using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingTool.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TicketingTool.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<int>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    [Required]
    [Display(Name = "Username")]
    public override string UserName { get; set; }

    public ICollection<ProjectUserRole> Projects { get; set; } = new List<ProjectUserRole>();

    public ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();

    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
}