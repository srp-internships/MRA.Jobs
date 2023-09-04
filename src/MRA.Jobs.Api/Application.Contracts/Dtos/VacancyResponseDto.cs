using MRA.Jobs.Application.Contracts.Dtos.Responses;

namespace MRA.Jobs.Application.Contracts.Dtos;
public class VacancyResponseDto
{
    public string Response { get; set; }
    public VacancyQuestionDto VacancyQuestion { get; set; }
}
