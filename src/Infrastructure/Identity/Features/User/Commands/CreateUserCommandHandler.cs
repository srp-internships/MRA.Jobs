using FluentValidation;
using MRA.Jobs.Infrastructure.Shared.Users.Commands;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotNull().NotEmpty();
        RuleFor(x => x.Roles).NotEmpty();
    }
}

public class CreateUserCommandHandler/* : IRequestHandler<CreateUserCommand, Unit>*/
{
    //private readonly UserManager<ApplicationUser> _userManager;

    //public CreateUserCommandHandler(UserManager<ApplicationUser> userManager)
    //{
    //    _userManager = userManager;
    //}

    //public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    //{
    //    var user = new ApplicationUser()
    //    {
    //        Id = request.Id,
    //        Email = request.Email,
    //        PhoneNumber = request.PhoneNumber,
    //        UserName = request.Email
    //    };

    //    foreach (var role in request.Roles)
    //        await _userManager.AddToRoleAsync(user, role);
    //    var password = request.Password;
    //    if (string.IsNullOrWhiteSpace(password))
    //        password = RandomPassword();
    //    return Unit.Value;
    //}

    public static string RandomPassword()
    {
        string[] randomChars = new[]
        {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz",
                "0123456789",
                "!@$?_-"
            };

        Random rand = new Random(Environment.TickCount);
        List<char> chars = new List<char>();

        chars.Insert(rand.Next(0, chars.Count),
            randomChars[0][rand.Next(0, randomChars[0].Length)]);

        chars.Insert(rand.Next(0, chars.Count),
            randomChars[1][rand.Next(0, randomChars[1].Length)]);

        chars.Insert(rand.Next(0, chars.Count),
            randomChars[2][rand.Next(0, randomChars[2].Length)]);

        chars.Insert(rand.Next(0, chars.Count),
            randomChars[3][rand.Next(0, randomChars[3].Length)]);

        for (int i = chars.Count; i < 8; i++)
        {
            string rcs = randomChars[rand.Next(0, randomChars.Length)];
            chars.Insert(rand.Next(0, chars.Count),
                rcs[rand.Next(0, rcs.Length)]);
        }

        return new string(chars.ToArray());
    }
}
