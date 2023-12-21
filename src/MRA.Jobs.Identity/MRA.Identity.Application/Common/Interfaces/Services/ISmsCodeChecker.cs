using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Identity.Application.Common.Interfaces.Services;
public interface ISmsCodeChecker
{
    bool VerifyPhone(int code, string phoneNumber);
}
