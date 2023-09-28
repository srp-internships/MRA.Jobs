namespace MRA.Jobs.Client.Identity;

public static class ClaimTypes
{
    private const string ClaimTypeNamespace = "http://schemas.microsoft.com/ws/2008/06/identity/claims";

    public const string Application = ClaimTypeNamespace + "/application";
    public const string Role = ClaimTypeNamespace + "/role";
    public const string Id = ClaimTypeNamespace + "/id";
    public const string Username = ClaimTypeNamespace + "/username";
    public const string Email = ClaimTypeNamespace + "/email";
}