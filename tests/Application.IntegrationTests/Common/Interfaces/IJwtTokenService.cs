using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Jobs.Application.IntegrationTests.Common.Interfaces;
public interface IJwtTokenService
{
    public string CreateTokenByClaims(IList<Claim> user);
}
