using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries;
public class GetTrainingVacancyBySlugQueryValidator : AbstractValidator<GetTrainingVacancyBySlugQuery>
{
    public GetTrainingVacancyBySlugQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
