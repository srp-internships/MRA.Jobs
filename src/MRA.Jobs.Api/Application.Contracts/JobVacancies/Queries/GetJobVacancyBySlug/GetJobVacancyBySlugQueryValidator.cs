
namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;

public class GetJobVacancyBySlugQueryValidator : AbstractValidator<GetJobVacancyBySlugQuery>
{
    public GetJobVacancyBySlugQueryValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}
