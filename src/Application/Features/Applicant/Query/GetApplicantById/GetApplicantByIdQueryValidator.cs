using MRA.Jobs.Application.Contracts.Applicant.Queries;

namespace MRA.Jobs.Application.Features.Applicant.Query.GetApplicantById;

public class GetApplicantByIdQueryValidator : AbstractValidator<GetApplicantByIdQuery>
{
    public GetApplicantByIdQueryValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
    }
}