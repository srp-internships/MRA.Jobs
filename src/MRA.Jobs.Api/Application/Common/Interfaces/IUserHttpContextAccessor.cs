namespace MRA.Jobs.Application.Common.Interfaces;

public interface IUserHttpContextAccessor
{
    Guid GetUserId();
    String GetUserName();

    List<string> GetUserRoles();
}