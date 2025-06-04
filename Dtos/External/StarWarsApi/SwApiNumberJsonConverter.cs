using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Dtos.External.StarWarsApi;

public class SwApiNumberJsonConverter : JsonConverter<SwApiNumber>
{
    public override SwApiNumber Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            // hopefully it's an int
            return new SwApiSingleNumber(reader.GetInt32());
        }
        
        if (reader.TokenType != JsonTokenType.String) throw new JsonException($"Unexpected token type: {reader.TokenType}");

        string? value = reader.GetString();

        // replace invalid values with a negative number
        if (string.IsNullOrWhiteSpace(value)) return new SwApiSingleNumber(-1);
        
        // if there are two values, split them and parse into int
        if (value.Contains('-'))
        {
            string[] splitResults = value.Split('-');
            if (splitResults.Length != 2)
                throw new JsonException($"Unexpected number of values in range: {splitResults.Length}");
            long firstValue = ParseLongAndThrowOnInvalid(splitResults[0]);
            long secondValue = ParseLongAndThrowOnInvalid(splitResults[1]);
            return new SwApiNumberRange(firstValue, secondValue);
        }

        if (value.Contains(".")) throw new JsonException($"Expecting an integer, got {value}");
        
        value = Regex.Replace(value, @"[^\d]", "");
        long singleValue = ParseLongAndThrowOnInvalid(value);

        return new SwApiSingleNumber(singleValue);
    }

    private long ParseLongAndThrowOnInvalid(string? input)
    {
        if(!long.TryParse(input, out long firstValue))
            throw new JsonException($"Expecting an integer: {input}");
        return firstValue;
    }

    public override void Write(Utf8JsonWriter writer, SwApiNumber value, JsonSerializerOptions options)
    {
        if (value is SwApiSingleNumber singleNumber)
        {
            writer.WriteStringValue(singleNumber.ToString());    
        } else if (value is SwApiNumberRange numberRange)
        {
            writer.WriteStringValue($"{numberRange.Start}-{numberRange.End}");
        }
        else
        {
            throw new NotImplementedException($"Serialization not implemented for type {value.GetType().Name}");
        }
    }
}