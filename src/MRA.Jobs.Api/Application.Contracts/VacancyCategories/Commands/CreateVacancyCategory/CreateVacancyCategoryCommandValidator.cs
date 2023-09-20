
namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands.CreateVacancyCategory;


public class CreateVacancyCategoryCommandValidator : AbstractValidator<CreateVacancyCategoryCommand>
{
    public CreateVacancyCategoryCommandValidator()
    {
        RuleFor(s => s.Name).NotEmpty();        
    }
}