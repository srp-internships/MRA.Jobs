namespace MRA.Jobs.Application.Contracts.Applications.Commands;
public class UpdateApplicationCommand:IRequest<Guid>
{
    public Guid Id { get; set; }
    public string CoverLetter { get; set; }
    public string CV { get; set; }
}
