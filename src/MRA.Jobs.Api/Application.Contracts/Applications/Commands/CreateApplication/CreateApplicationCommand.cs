using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

public class CreateApplicationCommand : IRequest<Guid>
{
    public string CoverLetter { get; set; }
    public Guid VacancyId { get; set; }
    public IEnumerable<VacancyResponseDto> VacancyResponses { get; set; }
    public IEnumerable<TaskResponseDto> TaskResponses { get; set; }
    
}