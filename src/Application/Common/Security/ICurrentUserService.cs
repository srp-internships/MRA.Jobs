namespace MRA.Jobs.Application.Common.Security;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string UserName { get; }
}
