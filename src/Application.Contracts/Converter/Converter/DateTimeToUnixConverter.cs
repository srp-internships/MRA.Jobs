using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MRA.Jobs.Application.Contracts.Converter.Converter;

public class DateTimeToUnixConverter : DateTimeConverterBase
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is not DateTime dateTime)
        {
            throw new Exception("Expected date object value.");
        }

        long unixTime = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        writer.WriteValue(unixTime);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.Integer)
        {
            throw new Exception(
                $"Unexpected token parsing date. Expected Integer, got {reader.TokenType}.");
        }

        long ticks = (long)(reader.Value ?? 0);
        DateTime date = new DateTime(1970, 1, 1);
        date = date.AddSeconds(ticks).ToLocalTime();
        return date;
    }
}