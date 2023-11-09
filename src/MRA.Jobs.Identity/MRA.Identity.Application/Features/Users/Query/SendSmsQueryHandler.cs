using MediatR;
using Microsoft.Extensions.Logging;
using MRA.Configurations.Common.Interfaces.Services;
using MRA.Configurations.OsonSms.SmsService;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.User.Queries;


namespace MRA.Identity.Application.Features.Users.Query;
public class SendSmsQueryHandler : IRequestHandler<SendVerificationCodeSmsQuery, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;
    private readonly ILogger<SmsService> _logger;

    public SendSmsQueryHandler(IApplicationDbContext context, ISmsService smsService, ILogger<SmsService> logger)
    {
        _context = context;
        _smsService = smsService;
        _logger = logger;
    }
    public async Task<bool> Handle(SendVerificationCodeSmsQuery request, CancellationToken cancellationToken)
    {
        request.PhoneNumber = request.PhoneNumber.Trim();
        if (request.PhoneNumber.Length == 9) request.PhoneNumber = "+992" + request.PhoneNumber.Trim();
        else if (request.PhoneNumber.Length == 12 && request.PhoneNumber[0] != '+') request.PhoneNumber = "+" + request.PhoneNumber;

        if (!System.Text.RegularExpressions.Regex.IsMatch(request.PhoneNumber, @"^\+992\d{9}$"))
        {
            throw new FormatException("Phone number does not follow the required format.");
        }

        var response = await _smsService.SendSmsAsync(request.PhoneNumber, GenerateMessage(out int code));

        if (!response) return false;

        try
        {
            _context.ConfirmationCodes.Add(new Domain.Entities.ConfirmationCode
            {
                Code = code,
                PhoneNumber = request.PhoneNumber,
            });
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }

    private static string GenerateMessage(out int code)
    {
        Random random = new Random();
        code = random.Next(1000, 10001);
        return $"Your confirmation code is: {code}. Please enter this code to verify your phone number.";
    }
}