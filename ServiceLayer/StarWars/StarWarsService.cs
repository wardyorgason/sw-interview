using Dtos.External.StarWarsApi;
using Dtos.StarWars;

namespace ServiceLayer.StarWars;

public interface StarWarsService
{
    Task<List<SwApiFilm>> GetAllFilms();
    Task<FilmBreakdownChartData> GetFilmBreakdownChartData();
    Task<List<SwApiStarship>> SearchStarships(string? searchString);
}