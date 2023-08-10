namespace MRA.Jobs.Application.Contracts.Applications.Commands;
public class UpdateApplicationStatus : IRequest<bool>
{
    public Guid Id { get; set; }
    public int StatusId { get; set; }
}
