namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands.DeleteVacancyCategory;

public class DeleteVacancyCategoryCommandValidator : AbstractValidator<DeleteVacancyCategoryCommand>
{
    public DeleteVacancyCategoryCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}