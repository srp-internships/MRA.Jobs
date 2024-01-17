using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Identity.Application.Contract.Messages.Commands;
public class SendMessageCommand : IRequest<bool>
{
    public string Phone { get; set; }
    public string Text { get; set; }
    public string Comment { get; set; }
}
