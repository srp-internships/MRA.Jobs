using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Application.Features.InternshipVacancies.Queries.GetInternshipById;
public class GetInternshipVacancyBySlugQueryValidator : AbstractValidator<InternshipVacancyResponse>
{
    public GetInternshipVacancyBySlugQueryValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}