using System.ComponentModel.Design;
using TicketingTool.Areas.Identity.Data;

namespace TicketingTool.Models
{
    public class Ticket
    {
        public int ID { get; set; }
        public string IssueKey { get; set; }
        public Project Project {  get; set; }
        public Component Component { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ApplicationUser Creator { get; set; }
        public ApplicationUser Assignee { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime ResolvedDate { get; set; }
    }
}
