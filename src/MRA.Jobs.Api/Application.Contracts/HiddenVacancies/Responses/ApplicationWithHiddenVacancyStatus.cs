using MRA.Jobs.Application.Contracts.Dtos.Enums;

namespace MRA.Jobs.Application.Contracts.HiddenVacancies.Responses;

public class ApplicationWithHiddenVacancyStatus
{
    public ApplicationStatusDto.ApplicationStatus Status { get; set; }
}