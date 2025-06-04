using Dtos.External.StarWarsApi;

namespace Dtos.StarWars;

public class FilmBreakdownChartData
{
    public List<(SwApiFilm Film, FilmBreakdownSpecifics Specifics)> Data { get; set; } = [];
}

public record FilmBreakdownSpecifics (int Starships, int People, int Planets);