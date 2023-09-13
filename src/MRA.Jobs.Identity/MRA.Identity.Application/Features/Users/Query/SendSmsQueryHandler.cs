using MediatR;
using Microsoft.Extensions.Logging;
using Mra.Shared.Common.Interfaces.Services;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.User.Queries;


namespace MRA.Identity.Application.Features.Users.Query;
public class SendSmsQueryHandler : IRequestHandler<SendSmsQuery, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;
    private readonly ILogger _logger;

    public SendSmsQueryHandler(IApplicationDbContext context,  ISmsService smsService, ILogger logger)
    {
        _context = context;
        _smsService = smsService;
        _logger = logger;
    }
    public async Task<bool> Handle(SendSmsQuery request, CancellationToken cancellationToken)
    {
        int code;
        var response = await _smsService.SendSmsAsync(request.PhoneNumber, GenerateMessage(out code));

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