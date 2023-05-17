using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;

namespace MRA.Jobs.Application.Features.VacancyCategories.Queries.GetVacancyCategoryById;

public class GetVacancyCategoryByIdQueryValidator : AbstractValidator<GetVacancyCategoryByIdQuery>
{
    public GetVacancyCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}