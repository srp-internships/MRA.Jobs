using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;

namespace MRA.Identity.Application.Services;
public class SmsCodeChecker : ISmsCodeChecker
{
    private readonly IApplicationDbContext _context;

    public SmsCodeChecker(IApplicationDbContext context)
    {
        _context = context;
    }
    public bool VerifyPhone(int code, string phoneNumber)
    {
        phoneNumber = phoneNumber.Trim();
        if (phoneNumber.Length == 9) phoneNumber = "+992" + phoneNumber.Trim();
        else if (phoneNumber.Length == 12 && phoneNumber[0] != '+')
            phoneNumber = "+" + phoneNumber;

        var expirationTime = DateTime.Now.AddMinutes(-1);

        var result = _context.ConfirmationCodes
            .Any(c => c.Code == code && c.PhoneNumber == phoneNumber && c.SentAt >= expirationTime);

        return result;
    }
}
