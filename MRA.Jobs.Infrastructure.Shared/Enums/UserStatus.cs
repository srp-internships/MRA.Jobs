using System.Text.Json.Serialization;

namespace MRA.Jobs.Infrastructure.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserStatus
{
    Active,
    WaitingVerification,
    Locked,
    Blocked
}