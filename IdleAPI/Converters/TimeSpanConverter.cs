using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdleAPI.Converters;

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    private TimeSpan ParseString(string? stringValue)
    {
        if (double.TryParse(stringValue, out var d))
        {
            return new TimeSpan((long)(d * TimeSpan.TicksPerMillisecond));
        }
        throw new JsonException($"String {stringValue} could not be converted to a TimeSpan");
    }

    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number when reader.TryGetDouble(out var b) => new TimeSpan((long)(b * TimeSpan.TicksPerMillisecond)),
            JsonTokenType.String => ParseString(reader.GetString()),
            _ => throw new JsonException($"Could not convert {reader.TokenType} to a TimeSpan")
        };
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.TotalMilliseconds);
    }
}
