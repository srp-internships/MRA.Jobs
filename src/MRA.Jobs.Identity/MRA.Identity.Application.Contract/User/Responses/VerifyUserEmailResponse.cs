using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Identity.Application.Contract.User.Responses;
public record VerifyUserEmailResponse
{
    public bool Success { get; set; }

    public string ErrorMessage { get; set; }
}
