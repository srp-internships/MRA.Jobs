using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MRA.Identity.Application.Contract.ApplicationRoles.Commands;
public class DeleteRoleCommand : IRequest<ApplicationResponse<bool>>
{
    public string Slug { get; set; }
}
