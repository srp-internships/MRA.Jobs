using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRA.Identity.Application.Common.Interfaces.Services;
public interface ISmsService
{
    Task<string> SendSms(string phoneNumber);
    Task<bool> CheckCode(int code);
}
