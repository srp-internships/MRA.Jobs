
namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands.UpdateVacancyCategory;

public class UpdateVacancyCategoryCommandValidator : AbstractValidator<UpdateVacancyCategoryCommand>
{
    public UpdateVacancyCategoryCommandValidator()
    {
        RuleFor(s => s.Slug).NotEmpty();
        RuleFor(s => s.Name).NotEmpty();
    }
}
