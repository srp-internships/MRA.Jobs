namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

public class UpdateVacancyCategoryCommand : IRequest<Guid>
{
    public string Slug { get; set; }
    public string Name { get; set; }
}