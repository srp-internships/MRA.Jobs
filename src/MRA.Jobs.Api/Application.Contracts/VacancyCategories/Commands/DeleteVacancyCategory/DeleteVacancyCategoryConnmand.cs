namespace MRA.Jobs.Application.Contracts.VacancyCategories.Commands.DeleteVacancyCategory;

public class DeleteVacancyCategoryCommand : IRequest<bool>
{
    public string Slug { get; set; }
}
