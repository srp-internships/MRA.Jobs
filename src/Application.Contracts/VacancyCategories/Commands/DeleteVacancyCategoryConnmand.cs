namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
public class DeleteVacancyCategoryCommand : IRequest<bool>
{
    public string Slug { get; set; }
}   
