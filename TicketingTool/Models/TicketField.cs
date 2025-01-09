using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using TicketingTool.Areas.Identity.Data;
namespace TicketingTool.Models
{ 
    public class TicketField
    {
        public int ID { get; set; }
        public string FieldName { get; set; }
        public string FieldDisplayName { get; set; }

        public static async Task<List<TicketField>> GetTicketFields(object ticket) 
        {
            Type type = ticket.GetType();
            var fields = new List<TicketField>();
            int idCounter = 1;

            foreach (var property in type.GetProperties())
            {
                if (property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() is DisplayAttribute displayAttr)
                {
                    fields.Add(new TicketField
                    {
                        ID = idCounter++,
                        FieldName = property.Name,
                        FieldDisplayName 
                            = displayAttr.Name ?? property.Name
                    });
                }
                else
                {
                    fields.Add(new TicketField
                    {
                        ID = idCounter++,
                        FieldName = property.Name,
                        FieldDisplayName = property.Name
                    });
                }
            }

            return fields;
        }
    }
}