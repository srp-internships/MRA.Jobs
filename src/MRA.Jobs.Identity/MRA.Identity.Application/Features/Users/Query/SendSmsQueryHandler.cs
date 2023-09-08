using MediatR;
using Mra.Shared.Common.Interfaces.Services;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Contract.User.Queries;


namespace MRA.Identity.Application.Features.Users.Query;
public class SendSmsQueryHandler : IRequestHandler<SendSmsQuery, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ISmsService _smsService;

    public SendSmsQueryHandler(IApplicationDbContext context, ISmsService smsService)
    {
        _context = context;
        _smsService = smsService;
    }
    public async Task<bool> Handle(SendSmsQuery request, CancellationToken cancellationToken)
    {
        var code = await _smsService.SendSmsAsync(request.PhoneNumber);
        _context.ConfirmationCodes.Add(new Domain.Entities.ConfirmationCode
        {
            Code = code,
            PhoneNumber = request.PhoneNumber,
        });
        await _context.SaveChangesAsync();

        return true;
    }
}