namespace MRA.Identity.Application.Common.Interfaces.Services;
public interface IUserHttpContextAccessor
{
    Guid GetUserId();
    String GetUserName();

    List<string> GetUserRoles();
}
