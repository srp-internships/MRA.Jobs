using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command.CreateVacancyCategory;

public class CreateVacancyCategoryCommandValidator : AbstractValidator<CreateVacancyCategoryCommand>
{
    public CreateVacancyCategoryCommandValidator()
    {
        RuleFor(s => s.Name).NotEmpty();
    }
}