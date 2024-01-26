using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Identity.Application.Contract.Messages.Responses;

namespace MRA.Identity.Application.Contract.Messages.Queries;
public class GetAllMessagesQuery : IRequest<List<GetMessageResponse>>
{
    public string SenderUserName { get; set; }
    public string Phone { get; set; }
}
