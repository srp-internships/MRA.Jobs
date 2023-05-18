using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.Applicant.Commands;
public class RemoveTagFromApplicantCommand : IRequest<bool>
{
    public Guid ApplicantId { get; set; }
    public Guid TagId { get; set; }
}
