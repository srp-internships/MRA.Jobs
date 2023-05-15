using MediatR;
using MRA.Jobs.Application.Contracts.Applicant.Responses;

namespace MRA.Jobs.Application.Contracts.Applicant.Queries;

public class GetAllApplicantQuery : IRequest<ApplicantListDto>
{
    public Guid Id { get; set; }
}