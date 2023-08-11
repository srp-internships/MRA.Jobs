using System.Text.Json.Serialization;

namespace MRA.Jobs.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExperienceLevel
{
    Trainee,
    Junior,
    Middle,
    Senior,
    Teamlead
}