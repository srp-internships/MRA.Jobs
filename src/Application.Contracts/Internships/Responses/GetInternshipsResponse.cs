namespace MRA.Jobs.Application.Contracts.Internships.Responses;
public class GetInternshipsResponse
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public int Duration { get; set; }
    public int Stipend { get; set; }
}
