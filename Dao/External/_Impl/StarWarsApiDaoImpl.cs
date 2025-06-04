using System.Text.Json;
using System.Text.Json.Serialization;
using Dtos.External.StarWarsApi;
using Utilities.Wrappers;

namespace Dao.External._Impl;

public class StarWarsApiDaoImpl : StarWarsApiDao
{
    private readonly HttpClientWrapper _httpClient;
    private readonly JsonSerializerOptions _options;
    public const string BASE_URL = "https://swapi.py4e.com/api";
    public StarWarsApiDaoImpl(HttpClientWrapper httpClient)
    {
        _httpClient = httpClient;
        _options = new(JsonSerializerDefaults.Web)
        {
            Converters = { new SwApiUrlJsonConverter() }
        };
    }

    public async Task<SwApiListingResult<SwApiFilm>?> ListAllFilms()
    {
        string filmsPath = "/films/";
        string body = await _httpClient.GetStringAsync($"{BASE_URL}{filmsPath}");
        return JsonSerializer.Deserialize<SwApiListingResult<SwApiFilm>>(body, _options);
    }

    public async Task<SwApiFilm?> GetFilmById(int id)
    {
        string filmsPath = $"/films/{id}/";
        string body = await _httpClient.GetStringAsync($"{BASE_URL}{filmsPath}");
        return JsonSerializer.Deserialize<SwApiFilm>(body, _options);
    }

    public async Task<SwApiListingResult<SwApiStarship>?> ListAllStarships()
    {
        string starshipsPath = "/starships/";
        string body = await _httpClient.GetStringAsync($"{BASE_URL}{starshipsPath}");
        return JsonSerializer.Deserialize<SwApiListingResult<SwApiStarship>>(body, _options);
    }
}