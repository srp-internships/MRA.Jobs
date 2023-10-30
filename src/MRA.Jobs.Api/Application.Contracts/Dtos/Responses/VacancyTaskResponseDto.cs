namespace MRA.Jobs.Application.Contracts.Dtos.Responses;
public class VacancyTaskResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Template { get; set; }
    public string Test { get; set; }
}
