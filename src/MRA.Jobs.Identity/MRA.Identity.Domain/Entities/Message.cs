using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Identity.Domain.Entities;
public class Message
{
    public Guid Id { get; set; }
    public string SenderUserName { get; set; }
    public string Phone { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public string Comment { get; set; }
}
