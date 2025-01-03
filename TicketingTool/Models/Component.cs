using System.ComponentModel.DataAnnotations;

namespace TicketingTool.Models
{
    public class Component
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string ComponentName { get; set; }
    }
}
