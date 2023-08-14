using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MRA.Jobs.Application.Contracts.Converter.Converter;

public class DateTimeToUnixConverter : DateTimeConverterBase
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is not DateTime dateTime)
            throw new Exception("Expected date object value.");
        
        long unixTime = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        writer.WriteValue(unixTime);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.Integer)
        {
            throw new Exception(
                String.Format("Unexpected token parsing date. Expected Integer, got {0}.",
                    reader.TokenType));
        }

        var ticks = (long) reader.Value;
        var date = new DateTime(1970, 1, 1);
        date = date.AddSeconds(ticks).ToLocalTime();
        return date;
    }   
}