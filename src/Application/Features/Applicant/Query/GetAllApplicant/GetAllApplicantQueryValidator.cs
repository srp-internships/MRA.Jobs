using MRA.Jobs.Application.Contracts.Applicant.Queries;

namespace MRA.Jobs.Application.Features.Applicant.Query.GetAllApplicant;

public class GetAllApplicantQueryValidator : AbstractValidator<GetAllApplicantQuery>
{
    public GetAllApplicantQueryValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
    }
}