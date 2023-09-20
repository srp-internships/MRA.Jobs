namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands.CreateVacancyCategory;

public class CreateVacancyCategoryCommand : IRequest<string>
{
    public string Name { get; set; }
}