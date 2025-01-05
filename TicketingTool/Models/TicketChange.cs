using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingTool.Areas.Identity.Data;
namespace TicketingTool.Models
{
    public class TicketChange
    {
        [Key]
        [Required]
        public int ChangeID { get; set; }
        [Required]
        [ForeignKey(nameof(Ticket))]
        public int TicketID { get; set; }
        public Ticket TicketRef { get; set; }
        [Required]
        [ForeignKey(nameof(TicketField))]
        public int ChangedFieldID {  get;set; }
        public TicketField ChangedFieldRef { get; set; }
        [Required]
        public string OldValue { get; set; }
        public string NewValue { get; set; } = string.Empty;
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string changedByID { get; set; }
        public ApplicationUser ChangedByRef { get; set; }
        [Required]
        public DateTime ChangedAt { get; set; }
    }
}