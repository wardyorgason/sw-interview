using Dao.External._Impl;
using Dtos.External.StarWarsApi;
using Moq;
using NUnit.Framework;
using Utilities.Wrappers;

namespace UnitTests.Dao.External;

[TestFixture]
public class StarWarsApiDaoImplTests
{
    private StarWarsApiDaoImpl _apiDao;
    
    private Mock<HttpClientWrapper> _httpClientMock;
    
    [SetUp]
    public void SetUp()
    {
        _httpClientMock = new(MockBehavior.Strict);
        _apiDao = new(_httpClientMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClientMock.Verify();
    }

    [Test]
    public async Task ListAllFilms__Deserializes_Properly()
    {
        // Arrange
        string url = "https://swapi.py4e.com/api/films/";
        string result = await File.ReadAllTextAsync($"External{Path.DirectorySeparatorChar}TestObjects{Path.DirectorySeparatorChar}ListAllFilms__Deserializes_Properly.json");
        
        // Act
        _httpClientMock.Setup(h => h.GetStringAsync(url))
            .ReturnsAsync(result)
            .Verifiable();
        SwApiListingResult<SwApiFilm>? finalResult = await _apiDao.ListAllFilms();
        
        // Assert
        Assert.That(finalResult, Is.Not.Null);
        Assert.That(finalResult?.Count, Is.EqualTo(7));
        Assert.That(finalResult?.Results[0].Title, Is.EqualTo("A New Hope"));
    }
    
    [Test]
    public async Task GetFilmById__Deserializes_Properly()
    {
        // Arrange
        int filmId = 1;
        string url = $"https://swapi.py4e.com/api/films/{filmId}/";
        string result = await File.ReadAllTextAsync($"External{Path.DirectorySeparatorChar}TestObjects{Path.DirectorySeparatorChar}GetFilmById__Deserializes_Properly.json");
    
        // Act
        _httpClientMock.Setup(h => h.GetStringAsync(url))
            .ReturnsAsync(result)
            .Verifiable();
        SwApiFilm? film = await _apiDao.GetFilmById(filmId);
    
        // Assert
        Assert.That(film, Is.Not.Null);
        Assert.That(film?.Title, Is.EqualTo("A New Hope"));
        Assert.That(film?.EpisodeId, Is.EqualTo(4));
        Assert.That(film?.Director, Is.EqualTo("George Lucas"));
        Assert.That(film?.ReleaseDate, Is.EqualTo("1977-05-25"));
    }
}