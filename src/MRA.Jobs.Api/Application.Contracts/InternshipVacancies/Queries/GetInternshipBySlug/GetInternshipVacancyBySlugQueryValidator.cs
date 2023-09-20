
namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
public class GetInternshipVacancyBySlugQueryValidator : AbstractValidator<GetInternshipVacancyBySlugQuery>
{
    public GetInternshipVacancyBySlugQueryValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}