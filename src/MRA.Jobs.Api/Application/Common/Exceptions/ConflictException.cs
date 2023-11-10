namespace MRA.Jobs.Application.Common.Exceptions;

public class ConflictException : Exception
{
    public string Details { get; set; }

    public ConflictException(string details)
    {
        Details = details;
    }
}