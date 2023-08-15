using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Identity.Application.Contract.Applicant.Commands;

namespace MRA.Identity.Application.Features.Applicants.Command.CreateApplicant;
public class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, Guid>
{
    public CreateApplicantCommandHandler(ApplicationDbContext context)
    {
        
    }
    public Task<Guid> Handle(CreateApplicantCommand request, CancellationToken cancellationToken)
    {
        
    }
}
