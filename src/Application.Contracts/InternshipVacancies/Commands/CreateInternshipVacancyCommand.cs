namespace MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
public class CreateInternshipVacancyCommand : IRequest<Guid>
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public int Duration { get; set; }
    public int Stipend { get; set; }
}
