using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.Tag.Commands;
public class UpdateTagCommand :IRequest<long>
{
    public long Id { get; set; }
    public string Name { get; set; }
}
