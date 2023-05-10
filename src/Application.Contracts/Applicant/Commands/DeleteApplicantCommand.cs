using MediatR;

namespace MRA.Jobs.Application.Contracts.Applicant.Commands;

public class DeleteApplicantCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}