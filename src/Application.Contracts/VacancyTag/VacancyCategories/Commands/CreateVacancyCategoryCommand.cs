namespace MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Commands;

public class CreateVacancyCategoryCommand : IRequest<Guid>
{
    public string Name { get; set; }
}