namespace MRA.Jobs.Application.Contracts.Applications.Commands.UpdateApplicationStatus;

public class UpdateApplicationStatusCommand : IRequest<bool>
{
    public string ApplicantUserName { get; set; }
    public string Slug { get; set; }
    public int StatusId { get; set; }
}