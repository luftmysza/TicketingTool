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
        [MaxLength(30, ErrorMessage = "Component name cannot exceed 30 characters.")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Component name must start with a capital letter and contain only letters.")]
        public string ComponentName { get; set; }

        [Required]
        public int ProjectID { get; set; }
        public Project ProjectRef { get; set; }
    }
}