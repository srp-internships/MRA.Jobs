namespace MRA.Jobs.Application.Contracts.Applications.Commands;
public class UpdateApplicationCommand : IRequest<Guid>
{
    public string Slug { get; set; }
    public string CoverLetter { get; set; }
    public string CV { get; set; }
}
