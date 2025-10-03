using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdleAPI.Converters;

public class BooleanConverter : JsonConverter<bool>
{
    private bool ParseString(string? stringValue)
    {
        return stringValue switch
        {
            "true" => true,
            "false" => false,
            _ => throw new JsonException($"String {stringValue} could not be converted to a boolean")
        };
    }

    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number when reader.TryGetByte(out var b) => b > 0,
            JsonTokenType.String => ParseString(reader.GetString()),
            _ => throw new JsonException($"Could not convert {reader.TokenType} to a boolean")
        };
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
