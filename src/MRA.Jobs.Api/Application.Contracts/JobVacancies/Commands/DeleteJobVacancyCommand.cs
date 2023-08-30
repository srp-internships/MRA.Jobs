namespace MRA.Jobs.Application.Contracts.JobVacancies.Commands;

public class DeleteJobVacancyCommand : IRequest<bool>
{
    public string Slug { get; set; }
}


