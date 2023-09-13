namespace MRA.Identity.Application.Common.Interfaces.Services;
public interface IUserHttpContextAccessor
{
    Guid GetUserId();
}
