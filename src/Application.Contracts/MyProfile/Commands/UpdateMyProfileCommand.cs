using MRA.Jobs.Application.Contracts.Users.Responses;

namespace MRA.Jobs.Application.Contracts.Users.Commands;
public class UpdateMyProfileCommand : IRequest<MyProfileResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Avatar { get; set; }
    public Gender Gender { get; set; }
}
