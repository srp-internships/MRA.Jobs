using MRA.Jobs.Application.Common.Converter;
using Newtonsoft.Json;

using MediatR;

namespace MRA.Jobs.Application.Contracts.Applicant.Commands;

public class UpdateApplicantCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Avatar { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    [JsonConverter(typeof(DateTimeToUnixConverter))]
    public DateTime DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}