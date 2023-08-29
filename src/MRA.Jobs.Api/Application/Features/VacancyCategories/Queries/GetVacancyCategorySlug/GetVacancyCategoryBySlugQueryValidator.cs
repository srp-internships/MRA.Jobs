using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryBySlug;

public class GetVacancyCategoryBySlugQueryValidator : AbstractValidator<GetVacancyCategoryBySlugQuery>
{
    public GetVacancyCategoryBySlugQueryValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}