using System.ComponentModel.DataAnnotations;

namespace TicketingTool.Models
{
    public class Component
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Component")]
        public string ComponentName { get; set; }
    }
}