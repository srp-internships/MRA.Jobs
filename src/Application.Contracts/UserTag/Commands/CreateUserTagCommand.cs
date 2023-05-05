using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.UserTag.Commands;
public class CreateUserTagCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid TagId { get; set; }
}
