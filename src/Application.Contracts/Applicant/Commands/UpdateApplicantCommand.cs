using MediatR;

namespace MRA.Jobs.Application.Contracts.Applicant.Commands;

public class UpdateApplicantCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
}