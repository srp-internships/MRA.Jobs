namespace MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

public class CreateVacancyTagCommand : IRequest<Guid>
{
    public Guid VacancyId { get; set; }
    public Guid TagId { get; set; }
}