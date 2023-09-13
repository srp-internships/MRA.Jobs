using MediatR;
using Mra.Shared.Common.Interfaces.Services;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.User.Queries;


namespace MRA.Identity.Application.Features.Users.Query;
public class SendSmsQueryHandler : IRequestHandler<SendVerificationCodeSmsQuery, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;

    public SendSmsQueryHandler(IApplicationDbContext context,  ISmsService smsService)
    {
        _context = context;
        _smsService = smsService;
    }
    public async Task<bool> Handle(SendVerificationCodeSmsQuery request, CancellationToken cancellationToken)
    {
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