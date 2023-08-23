namespace MRA.Jobs.Infrastructure.Shared.Pemission.Responces;

public class PermissionResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
    public string DisplayName { get; set; }
    public string GroupDisplayName { get; set; }
}