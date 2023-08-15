using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;

public class GetTrainingVacancyByIdQueryValidator : AbstractValidator<GetTrainingVacancyBySlugQuery>
{
    public GetTrainingVacancyByIdQueryValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}