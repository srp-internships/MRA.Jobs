using MediatR;

namespace MRA.Jobs.Application.Contracts.VacancyTag.Commands;
public class CreateVacancyTagCommand : IRequest<Guid>
{
    public Guid VacancyId { get; set; }
    public Guid TagId { get; set; }
}
