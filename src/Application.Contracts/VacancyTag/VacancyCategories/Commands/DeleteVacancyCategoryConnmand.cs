namespace MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Commands;

public class DeleteVacancyCategoryCommand : IRequest<bool>
{
    public string Slug { get; set; }
}
