using MediatR;
using MRA.Jobs.Application.Contracts.Applicant.Responses;

namespace MRA.Jobs.Application.Contracts.Applicant.Queries;

public class GetApplicantByIdQuery : IRequest<ApplicantDetailsDto>
{
    public Guid Id { get; set; }  
}