using System.Text.Json.Serialization;

namespace Dtos.External.StarWarsApi;

public class SwApiStarship
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; }

    [JsonPropertyName("cost_in_credits")]
    public string CostInCredits { get; set; }

    [JsonPropertyName("length")]
    public string Length { get; set; }

    [JsonPropertyName("max_atmosphering_speed")]
    public string MaxAtmospheringSpeed { get; set; }

    [JsonPropertyName("crew")]
    public string Crew { get; set; }

    [JsonPropertyName("passengers")]
    public string Passengers { get; set; }

    [JsonPropertyName("cargo_capacity")]
    public string CargoCapacity { get; set; }

    [JsonPropertyName("consumables")]
    public string Consumables { get; set; }

    [JsonPropertyName("hyperdrive_rating")]
    public string HyperdriveRating { get; set; }

    [JsonPropertyName("MGLT")]
    public string MGLT { get; set; }

    [JsonPropertyName("starship_class")]
    public string StarshipClass { get; set; }

    [JsonPropertyName("pilots")]
    public List<SwApiUrl> PilotUrls { get; set; }

    [JsonPropertyName("films")]
    public List<SwApiUrl> FilmUrls { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("edited")]
    public DateTime Edited { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}