using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
public class GetTrainingCategoriesByCategoryNameQuery : IRequest<TrainingVacancyWithCategoryDto>
{
    public string CategorySlug { get; set; }
}
