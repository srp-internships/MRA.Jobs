namespace MRA.Jobs.Application.Contracts.Internships.Commands;
public class DeleteInternshipVacancyCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
