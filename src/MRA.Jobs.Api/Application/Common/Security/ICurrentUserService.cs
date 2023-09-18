namespace MRA.Jobs.Application.Common.Security;

public interface ICurrentUserService
{
    Guid? GetUserId();
    string GetEmail();
}