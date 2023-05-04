using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.User.Command;

namespace MRA.Jobs.Web.Controllers;

public class UserController : ApiControllerBase
{
    public UserController()
    {
        
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreateNewUser(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }
}