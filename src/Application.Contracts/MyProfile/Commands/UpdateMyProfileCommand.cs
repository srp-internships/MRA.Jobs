using MRA.Jobs.Application.Contracts.Converter.Converter;
using MRA.Jobs.Application.Contracts.MyProfile.Responses;
using Newtonsoft.Json;

namespace MRA.Jobs.Application.Contracts.MyProfile.Commands;

public class UpdateMyProfileCommand : IRequest<MyProfileResponse>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }

    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime DateOfBirth { get; set; }

    public string Avatar { get; set; }
    public Gender Gender { get; set; }
}