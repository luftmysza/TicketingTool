using System.ComponentModel.DataAnnotations;

namespace TicketingTool.Models
{
    public class Project
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string ProjectName { get; set; }
    }
}
