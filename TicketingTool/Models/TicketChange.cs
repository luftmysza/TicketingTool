using TicketingTool.Areas.Identity.Data;
namespace TicketingTool.Models
{
    public class TicketChange
    {
        public int ChangeID { get; set; }
        public int TicketID { get; set; }
        public TicketField ChangedField { get; set; }
        public ApplicationUser ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

    }
}
