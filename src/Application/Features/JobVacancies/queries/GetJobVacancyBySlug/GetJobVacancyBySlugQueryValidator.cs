using MRA.Jobs.Application.Contracts.JobVacancies.Queries;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyById;

public class GetJobVacancyBySlugQueryValidator : AbstractValidator<GetJobVacancyBySlugQuery>
{
    public GetJobVacancyBySlugQueryValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}
