using MRA.Jobs.Application.Contracts.TrainingModels.Queries;

namespace MRA.Jobs.Application.Features.TrainingVacancies.Queries.GetTrainingModelById;
public class GetTrainingVacancyByIdQueryValidator : AbstractValidator<GetTrainingVacancyByIdQuery>
{
    public GetTrainingVacancyByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
