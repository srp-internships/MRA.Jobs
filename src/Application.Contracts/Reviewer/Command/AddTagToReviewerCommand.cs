using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Jobs.Application.Contracts.Reviewer.Commands;
public class AddTagToReviewerCommand : IRequest<bool>
{
    public Guid ReviewerId { get; set; }
    public Guid TagId { get; set; }
}
