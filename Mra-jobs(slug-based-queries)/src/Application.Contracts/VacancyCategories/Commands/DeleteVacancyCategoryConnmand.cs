namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
public class DeleteVacancyCategoryCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}   
