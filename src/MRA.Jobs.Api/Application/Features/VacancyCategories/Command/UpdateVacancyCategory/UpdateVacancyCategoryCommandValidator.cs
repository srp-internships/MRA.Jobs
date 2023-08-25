using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command.UpdateVacancyCategory;

public class UpdateVacancyCategoryCommandValidator : AbstractValidator<UpdateVacancyCategoryCommand>
{
    public UpdateVacancyCategoryCommandValidator()
    {
        RuleFor(s => s.Slug).NotEmpty();
        RuleFor(s => s.Name).NotEmpty();
    }
}
