namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;
public class DeleteInternshipVacancyCommand : IRequest<bool>
{
    public string Slug { get; set; }
}
