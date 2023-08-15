using MRA.Jobs.Application.Contracts.JobVacancies.Queries;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyById;

public class GetJobVacancyByIdQueryValidator : AbstractValidator<GetJobVacancyBySlugQuery>
{
    public GetJobVacancyByIdQueryValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}