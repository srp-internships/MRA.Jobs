using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Queries;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryById;

public class GetVacancyCategoryBySlugQueryValidator : AbstractValidator<GetVacancyCategoryBySlugQuery>
{
    public GetVacancyCategoryBySlugQueryValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}