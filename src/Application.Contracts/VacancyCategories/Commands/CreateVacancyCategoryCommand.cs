namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
public class CreateVacancyCategoryCommand:IRequest<Guid>
{
    public string Name { get; set; }
}
    