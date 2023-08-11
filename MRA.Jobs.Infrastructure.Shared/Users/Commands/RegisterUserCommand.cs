using MediatR;
using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Infrastructure.Shared.Users.Commands;

public class RegisterUserCommand : IRequest<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public Gender Gender { get; set; }
    public int PhoneVerificationCode { get; set; }
    public bool TermsAccepted { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }
}