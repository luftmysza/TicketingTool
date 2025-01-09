using System.ComponentModel;
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
        [DisplayName("Changed Field")]
        public int ChangedFieldId {  get; set; }
        public string ChangedFieldName { get; set; }
        [DisplayName("Old Value")]
        public string? OldValue { get; set; }
        [DisplayName("New Value")]
        public string? NewValue { get; set; }
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        [DisplayName("Changed By")]
        public string ChangedBy { get; set; }
        public ApplicationUser ChangedByRef { get; set; }
        [Required]
        [DisplayName("Changed At")]
        public DateTime ChangedAt { get; set; }
    }
}