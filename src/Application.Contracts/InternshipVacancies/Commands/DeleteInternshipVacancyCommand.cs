namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;

public class DeleteInternshipVacancyCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}