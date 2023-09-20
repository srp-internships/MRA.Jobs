
namespace MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
public class GetTrainingVacancyBySlugQueryValidator : AbstractValidator<GetTrainingVacancyBySlugQuery>
{
    public GetTrainingVacancyBySlugQueryValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}
