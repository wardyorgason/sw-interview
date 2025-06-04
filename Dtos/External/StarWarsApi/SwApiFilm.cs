using System.Text.Json.Serialization;

namespace Dtos.External.StarWarsApi;

public class SwApiFilm
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("episode_id")]
    public int EpisodeId { get; set; }

    [JsonPropertyName("opening_crawl")]
    public string OpeningCrawl { get; set; }

    [JsonPropertyName("director")]
    public string Director { get; set; }

    [JsonPropertyName("producer")]
    public string Producer { get; set; }

    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; }  // Use DateTime if parsing to date

    [JsonPropertyName("characters")]
    public List<string> CharacterUrls { get; set; }

    [JsonPropertyName("planets")]
    public List<string> PlanetUrls { get; set; }

    [JsonPropertyName("starships")]
    public List<string> StarshipUrls { get; set; }

    [JsonPropertyName("vehicles")]
    public List<string> VehicleUrls { get; set; }

    [JsonPropertyName("species")]
    public List<string> SpeciesUrls { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("edited")]
    public DateTime Edited { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }
}