using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.Applications.Commands;
public class DeleteApplicationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
