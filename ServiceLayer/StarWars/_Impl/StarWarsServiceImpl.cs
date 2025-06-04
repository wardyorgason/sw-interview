using Dao.External;
using Dtos.External.StarWarsApi;
using Dtos.StarWars;

namespace ServiceLayer.StarWars._Impl;

public class StarWarsServiceImpl : StarWarsService
{
    private readonly StarWarsApiDao _apiDao;
    public StarWarsServiceImpl(StarWarsApiDao apiDao)
    {
        _apiDao = apiDao;
    }
    public async Task<List<SwApiFilm>> GetAllFilms()
    {
        SwApiListingResult<SwApiFilm>? films = await _apiDao.ListAllFilms();
        return films?.Results ?? [];
    }

    public async Task<FilmBreakdownChartData> GetFilmBreakdownChartData()
    {
        SwApiListingResult<SwApiFilm>? films = await _apiDao.ListAllFilms();
        if (films is null)
            throw new NullReferenceException("Unable to retrieve films from API");

        FilmBreakdownChartData chartData = new();
        foreach (SwApiFilm film in films.Results)
        {
            chartData.Data.Add((film, new FilmBreakdownSpecifics(film.StarshipUrls.Count, film.CharacterUrls.Count, film.PlanetUrls.Count)));
        }

        return chartData;
    }
}