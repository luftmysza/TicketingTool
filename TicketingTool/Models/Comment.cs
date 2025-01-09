using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketingTool.Areas.Identity.Data;

namespace TicketingTool.Models
{

    public class Comment
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string IssueKey { get; set; }

        [ForeignKey("IssueKey")]
        public Ticket TicketRef { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        [Required]
        public string AuthorUserName { get; set; }

        public ApplicationUser AuthorRef { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }

}
