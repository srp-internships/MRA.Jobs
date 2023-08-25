using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
public record GetTrainingCategoriesQuery : IRequest<List<TrainingCategoriesResponce>>
{
    public bool CheckDate { get; set; }
}
