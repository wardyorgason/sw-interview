using System.Text.Json;
using System.Text.Json.Serialization;
using Dtos.External.StarWarsApi;
using Utilities.Wrappers;

namespace Dao.External._Impl;

public class StarWarsApiDaoImpl : StarWarsApiDao
{
    private readonly HttpClientWrapper _httpClient;
    public const string BASE_URL = "https://swapi.py4e.com/api";
    public StarWarsApiDaoImpl(HttpClientWrapper httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SwApiListingResult<SwApiFilm>?> ListAllFilms()
    {
        string filmsPath = "/films/";
        string body = await _httpClient.GetStringAsync($"{BASE_URL}{filmsPath}");
        return JsonSerializer.Deserialize<SwApiListingResult<SwApiFilm>>(body);
    }

    public async Task<SwApiFilm?> GetFilmById(int id)
    {
        string filmsPath = $"/films/{id}/";
        string body = await _httpClient.GetStringAsync($"{BASE_URL}{filmsPath}");
        return JsonSerializer.Deserialize<SwApiFilm>(body);
    }
}