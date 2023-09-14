using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Identity.Domain.Entities;
public class ConfirmationCode
{
    public Guid Id { get; set; }
    public DateTime SentAt { get; set; } = DateTime.Now;
    public int Code { get; set; }
    public string PhoneNumber { get; set; }
}
