namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands.UpdateVacancyCategory;

public class UpdateVacancyCategoryCommand : IRequest<string>
{
    public string Slug { get; set; }
    public string Name { get; set; }
}