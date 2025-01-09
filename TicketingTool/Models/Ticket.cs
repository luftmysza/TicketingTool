using Microsoft.Identity.Client;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations;
using TicketingTool.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using TicketingTool.Data;

namespace TicketingTool.Models
{
    public class Ticket
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Issue Key")]
        [MaxLength(50)]
        public string IssueKey { get; set; }
        [Required]
        [ForeignKey(nameof(Project))]
        public int ProjectID { get; set; }
        [Display(Name = "Project")]
        public Project? ProjectRef { get; set; }
        [Required]
        [ForeignKey(nameof(Component))]
        public int ComponentID { get; set; }
        [Display(Name = "Component")]
        public Component? ComponentRef { get; set; }
        [StringLength(50)]
        public string? Title { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        [Required]
        [ForeignKey(nameof(Status))]        
        public int StatusID { get; set; }
        [Display(Name = "Status")]
        public Status? StatusRef { get; set; }
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string CreatorID { get; set; }
        [Display(Name = "Creator")]
        public ApplicationUser? CreatorRef { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string? AssigneeID { get; set; }
        [Display(Name = "Assignee")]
        public ApplicationUser? AssigneeRef { get; set; }
        [Required]
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }
        [Required]
        [Display(Name = "Last Updated Date")]
        public DateTime? LastUpdatedDate { get; set; }
        [Display(Name = "Resolved Date")]
        public DateTime? ResolvedDate { get; set; } = null;
        public ICollection<TicketChange> Changes { get; set; } = new List<TicketChange>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public static bool Exists(ApplicationDBContext context, string issueKey)
        {
            object? obj = context.Ticket.FirstOrDefault(t => t.IssueKey == issueKey);

            if (obj is null) return true;

            return false;
        }
    }
}
