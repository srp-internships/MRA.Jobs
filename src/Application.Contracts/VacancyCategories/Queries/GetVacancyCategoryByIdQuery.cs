namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
public class GetVacancyCategoryByIdQuery : IRequest<VacancyCategoryListDTO>
{
    public Guid Id { get; set; }
}
