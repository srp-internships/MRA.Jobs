namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

public class CreateVacancyCategoryCommand : IRequest<string>
{
    public string Name { get; set; }
}