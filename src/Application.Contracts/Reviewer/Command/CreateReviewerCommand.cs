using MRA.Jobs.Application.Common.Converter;
using Newtonsoft.Json;

using MediatR;

namespace MRA.Jobs.Application.Contracts.Reviewer.Command;

public class CreateReviewerCommand : IRequest<Guid>
{
    public string Avatar { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime DateOfBrith { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
}