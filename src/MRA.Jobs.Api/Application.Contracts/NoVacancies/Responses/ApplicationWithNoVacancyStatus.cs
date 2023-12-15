using MRA.Jobs.Application.Contracts.Dtos.Enums;

namespace MRA.Jobs.Application.Contracts.NoVacancies.Responses;

public class ApplicationWithNoVacancyStatus
{
    public bool Applied { get; set; } = true;
    public ApplicationStatusDto.ApplicationStatus? Status { get; set; }
}