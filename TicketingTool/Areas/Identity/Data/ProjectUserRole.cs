using System;
using Microsoft.AspNetCore.Identity;
using TicketingTool.Models;
using System.ComponentModel.DataAnnotations;

namespace TicketingTool.Areas.Identity.Data;

public class ProjectUserRole : IdentityUserRole<string>
{
    [Key]
    [Required]
    public int ProjectId { get; set; }
    public Project ProjectRef { get; set; }
    [Key]
    [Required]
    public override string UserId { get; set; }
    public ApplicationUser UserNameRef { get; set; }

    //RoleId is inherited
}
