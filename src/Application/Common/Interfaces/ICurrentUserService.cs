namespace MRA.Jobs.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
}
