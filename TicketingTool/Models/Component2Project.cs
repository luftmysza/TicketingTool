using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingTool.Models
{
    public class Component2Project
    {
        [ForeignKey(nameof(Component))]
        public int ComponentID { get; set; }
        public Component ComponentRef { get; set; }
        [ForeignKey(nameof(Project))]
        public int ProjectID { get; set; }
        public Project ProjectRef { get; set; }
    }
}
