using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command.DeleteVacancyCategory

;

public class DeleteVacancyCategoryCommandValidator : AbstractValidator<DeleteVacancyCategoryCommand>
{
    public DeleteVacancyCategoryCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}