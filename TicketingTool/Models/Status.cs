using System.ComponentModel.DataAnnotations;

namespace TicketingTool.Models
{
    public class Status
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Status")]
        public string StatusName { get; set; }
    }
}