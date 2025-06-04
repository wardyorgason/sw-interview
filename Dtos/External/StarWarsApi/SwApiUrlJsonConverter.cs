using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dtos.External.StarWarsApi;

public class SwApiUrlJsonConverter : JsonConverter<SwApiUrl>
{
    public override SwApiUrl Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string");
        }

        string? url = reader.GetString();

        // for now, just replace invalid values with an empty string
        if (string.IsNullOrWhiteSpace(url)) url = "";
        
        return new SwApiUrl { Url = url };
    }

    public override void Write(Utf8JsonWriter writer, SwApiUrl value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Url);
    }
}