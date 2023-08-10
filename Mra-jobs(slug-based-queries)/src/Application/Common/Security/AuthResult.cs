namespace MRA.Jobs.Application.Common.Security;

public class AuthResult
{
    internal AuthResult(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public bool Succeeded { get; set; }

    public string[] Errors { get; set; }

    public static AuthResult Success()
    {
        return new AuthResult(true, Array.Empty<string>());
    }

    public static AuthResult Failure(IEnumerable<string> errors)
    {
        return new AuthResult(false, errors);
    }
}
