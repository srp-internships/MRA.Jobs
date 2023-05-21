using System.Text.Json.Serialization;

namespace MRA.Jobs.Infrastructure.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserType
{
    Applicant = 0,
    Worker = 1
}
