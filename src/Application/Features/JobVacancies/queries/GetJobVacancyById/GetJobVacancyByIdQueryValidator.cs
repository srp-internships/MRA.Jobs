using MRA.Jobs.Application.Contracts.JobVacancies.Queries;

namespace MRA.Jobs.Application.Features.JobVacancies.queries.GetJobVacancyById;

public class GetJobVacancyByIdQueryValidator : AbstractValidator<GetJobVacancyByIdQuery>
{
    public GetJobVacancyByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}