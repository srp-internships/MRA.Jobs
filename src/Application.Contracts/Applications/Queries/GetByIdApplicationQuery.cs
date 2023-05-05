using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Contracts.Applications.Queries;
public class GetByIdApplicationQuery : IRequest<ApplicationResponse>
{
    public int Id { get; set; }
}
