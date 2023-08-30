namespace MRA.Identity.Application.Contract;

public abstract class ApplicationResponse<TResponse>
{
    public TResponse? Response { get; protected set; }
    public bool IsSuccess { get; protected set; } = true;
    public string? ErrorMessage { get; protected set; }
    public Exception? Exception { get; protected set; }
}

public abstract class ApplicationResponse
{
    public bool IsSuccess { get; protected set; } = true;
    public string? ErrorMessage { get; protected set; }
    public Exception? Exception { get; protected set; }
}