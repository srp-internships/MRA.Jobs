namespace MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;

public class UpdateApplicationStatus : IRequest<bool>
{
    public string Slug { get; set; }
    public int StatusId { get; set; }
}