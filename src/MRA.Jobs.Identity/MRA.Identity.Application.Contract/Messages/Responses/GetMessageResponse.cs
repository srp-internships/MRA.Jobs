using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRA.Identity.Application.Contract.Enums;

namespace MRA.Identity.Application.Contract.Messages.Responses;
public class GetMessageResponse
{
    public string SenderUserName { get; set; }
    public string Phone { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public string Comment { get; set; }
    public MessageStatusDto Status { get; set; }

}
