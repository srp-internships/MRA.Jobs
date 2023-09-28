using MRA.Identity.Application.Contract.User.Commands.RegisterUser;

namespace MRA.Jobs.Client.Identity;

public class CustomRegisterCommand:RegisterUserCommand
{
    public string ConfirmPassword { get; set; }
}