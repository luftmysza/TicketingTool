using System.ComponentModel.DataAnnotations;
using TicketingTool.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingTool.Models
{
    public class Project2Role2User
    {
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }
        public Project ProjectRef { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public int UserID { get; set; }
        public ApplicationUser UserRef { get; set; }
        [ForeignKey(nameof(IdentityRole<int>))]
        public int RoleID { get; set; }
        public IdentityRole<int> RoleRef { get; set; }
    }
}
