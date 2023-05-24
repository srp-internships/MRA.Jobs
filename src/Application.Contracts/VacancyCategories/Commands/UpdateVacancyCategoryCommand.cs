namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

public class UpdateVacancyCategoryCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
