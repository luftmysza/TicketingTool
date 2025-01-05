using System.Diagnostics.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingTool.Areas.Identity.Data;

namespace TicketingTool.Models
{
    public class TicketField
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        [ForeignKey(nameof(Project))]
        public int ProjectID { get; set; }
        public Project ProjectRef { get; set; }
        [Required]
        [StringLength(20)]
        public string FieldName { get; set; }
    }
}
