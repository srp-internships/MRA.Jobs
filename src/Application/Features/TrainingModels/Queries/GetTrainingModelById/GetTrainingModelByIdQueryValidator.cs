using MRA.Jobs.Application.Contracts.TrainingModels.Queries;

namespace MRA.Jobs.Application.Features.TrainingModels.Queries.GetTrainingModelById;
public class GetTrainingModelByIdQueryValidator : AbstractValidator<GetTrainingModelByIdQuery>
{
    public GetTrainingModelByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
