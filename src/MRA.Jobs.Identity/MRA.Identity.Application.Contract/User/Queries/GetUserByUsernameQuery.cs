using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Identity.Application.Contract.User.Queries;
public class GetUserByUsernameQuery : IRequest<UserResponse>
{
    public string UserName { get; set; }
}
