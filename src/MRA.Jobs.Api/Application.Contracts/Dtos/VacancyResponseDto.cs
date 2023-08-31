namespace MRA.Jobs.Application.Contracts.Dtos;
public class VacancyResponseDto
{
    public Guid Id { get; set; }
    public string Response { get; set; }
    public Guid ApplicationId { get; set; }
    public VacancyQuestionDto Question { get; set; }
}
