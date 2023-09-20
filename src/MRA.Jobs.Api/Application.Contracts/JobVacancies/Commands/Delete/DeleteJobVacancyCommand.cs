namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands.DeleteJobVacancy
    ;

public class DeleteJobVacancyCommand : IRequest<bool>
{
    public string Slug { get; set; }
}


