using Dtos.External.StarWarsApi;

namespace Dao.External;

public interface StarWarsApiDao
{
    public Task<SwApiListingResult<SwApiFilm>?> ListAllFilms();
    public Task<SwApiFilm?> GetFilmById(int id);
}