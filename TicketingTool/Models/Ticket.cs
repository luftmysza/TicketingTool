using Microsoft.Identity.Client;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations;
using TicketingTool.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingTool.Models
{
    public class Ticket
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string IssueKey { get; set; }
        [Required]
        [ForeignKey(nameof(Project))]
        public int ProjectID { get; set; }
        public Project ProjectRef { get; set; }
        [Required]
        [ForeignKey(nameof(Component))]
        public int ComponentID { get; set; }
        public Component ComponentRef { get; set; }
        [StringLength(50)]
        public string? Title { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        [Required]
        [ForeignKey(nameof(Status))]
        public int StatusID { get; set; }
        public Status StatusRef { get; set; }
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string CreatorID { get; set; }
        public ApplicationUser CreatorRef { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string? AssigneeID { get; set; }
        public ApplicationUser AssigneeRef { get; set; }
        [Required]
        public DateTime? CreatedDate { get; set; }
        [Required]
        public DateTime? LastUpdatedDate { get; set; }
        public DateTime? ResolvedDate { get; set; } = null;
        public ICollection<TicketChange> Changes { get; set; } = new HashSet<TicketChange>();
    }
}
