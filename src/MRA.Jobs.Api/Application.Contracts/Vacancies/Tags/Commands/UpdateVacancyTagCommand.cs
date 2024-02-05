namespace MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

public class UpdateVacancyTagCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid VacancyId { get; set; }
    public Guid TagId { get; set; }
}