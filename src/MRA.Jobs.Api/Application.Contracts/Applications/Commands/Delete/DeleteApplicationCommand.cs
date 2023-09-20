namespace MRA.Jobs.Application.Contracts.Applications.Commands.Delete;

public class DeleteApplicationCommand : IRequest<bool>
{
    public string Slug { get; set; }
}
