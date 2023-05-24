//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using MRA.Identity.Contract.Pemission;
//using MRA.Identity.Contract.Role.Requests;
//using MRA.Identity.Contract.Role.Responses;
//using MRA.Jobs.Shared.Contracts.PaggedList;

//namespace MRA.Jobs.API.ControllersAuth;
//public class RolesController : ApiControllerBase
//{
//    private readonly IMediator _mediator;

//    public RolesController(IMediator mediator)
//    {
//        _mediator = mediator;
//    }

//    [HttpGet]
//    public async Task<ActionResult<PaggedList<RoleResponse>>> Get([FromQuery] PaggedListQuery<RoleResponse> request)
//    {
//        return Ok(await _mediator.Send(request));
//    }

//    [HttpPost]
//    public async Task<ActionResult<RoleResponse>> Create(CreateRoleCommand request)
//    {
//        return Ok(await _mediator.Send(request));
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Deleet([FromRoute] DeleteRoleCommand request)
//    {
//        await _mediator.Send(request);
//        return NoContent();
//    }

//    [HttpPut("{id}")]
//    public async Task<ActionResult<RoleResponse>> Put([FromRoute] Guid id, [FromBody] UpdateRoleCommand request)
//    {
//        request.Id = id;
//        return Ok(await _mediator.Send(request));
//    }

//    [HttpGet("{id}/permissions")]
//    public async Task<ActionResult<PermissionResponse[]>> GetPermission(Guid id)
//    {
//        return Ok(await _mediator.Send(new GetRolePermissionsQuery() { Id = id }));
//    }

//    [HttpPut("{id}/permissions/grant")]
//    public async Task<ActionResult<RoleResponse>> GrantPermission([FromRoute] Guid id, [FromBody] RoleGrantPermissionCommand request)
//    {
//        request.Id = id;
//        return Ok(await _mediator.Send(request));
//    }

//    [HttpPut("{id}/permissions/revoke")]
//    public async Task<ActionResult<RoleResponse>> RevokePermission([FromRoute] Guid id, [FromBody] RoleRevokePermissionCommand request)
//    {
//        request.Id = id;
//        return Ok(await _mediator.Send(request));
//    }
//}
