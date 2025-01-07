using System.ComponentModel.DataAnnotations;
using TicketingTool.Areas.Identity.Data;

namespace TicketingTool.Models
{
    public class Project
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Project Key")]
        public string ProjectKey { get; set; }

        public int Counter { get; set; }

        [Required]
        [Display(Name = "Project")]
        public string ProjectName { get; set; }

        public ICollection<ApplicationUser> PMs { get; set; } = new List<ApplicationUser>();
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ICollection<Component> Components { get; set; } = new List<Component>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}