using MediatR;

namespace MRA.Jobs.Application.Contracts.Reviewer.Command;

public class UpdateReviewerCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
}