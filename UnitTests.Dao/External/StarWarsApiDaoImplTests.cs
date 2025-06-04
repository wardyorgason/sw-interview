using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Dao.External._Impl;
using Dtos.External.StarWarsApi;
using Moq;
using NUnit.Framework;
using Utilities.Wrappers;

namespace UnitTests.Dao.External;

[TestFixture]
public class StarWarsApiDaoImplTests
{
    public StarWarsApiDaoImplTests()
    {
        SetUpJsonDeserializer();
    }
    
    private StarWarsApiDaoImpl _apiDao = null!;
    
    private Mock<HttpClientWrapper> _httpClientMock = null!;
    
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

    [Test]
    public async Task ListAllStarships__Deserializes_Properly()
    {
        // Arrange
        string url = "https://swapi.py4e.com/api/starships/";
        string result = await File.ReadAllTextAsync($"External{Path.DirectorySeparatorChar}TestObjects{Path.DirectorySeparatorChar}ListAllStarships__Deserializes_Properly.json");
        
        // Act
        _httpClientMock.Setup(h => h.GetStringAsync(url))
            .ReturnsAsync(result)
            .Verifiable();
        SwApiListingResult<SwApiStarship>? finalResult = await _apiDao.ListAllStarships();
        
        // Assert
        Assert.That(finalResult, Is.Not.Null);
        Assert.That(finalResult?.Count, Is.EqualTo(37));
        Assert.That(finalResult?.Results[0].Model, Is.EqualTo("CR90 corvette"));
        Assert.That(finalResult?.Results[0].FilmUrls[0].Url, Is.EqualTo("https://swapi.py4e.com/api/films/1/"));
        
        // make sure the Id getter works on the SwApiUrl type
        Assert.That(finalResult?.Results[0].FilmUrls[0].Id, Is.EqualTo(1));
    }


    private void SetUpJsonDeserializer()
    {
        // This is some reflection BS to insert a global JsonConverter for tests
        // this is normally handled by .Net in the Program.cs file, but that doesn't
        // carry into unit tests
        // See https://stackoverflow.com/questions/58331479/how-to-globally-set-default-options-for-system-text-json-jsonserializer/74741382#74741382
        List<JsonConverter> jsonConverterList = new()
        {
            new SwApiUrlJsonConverter(),
        };

        Type type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .SingleOrDefault(t => t.FullName == "System.Text.Json.JsonSerializerOptions+ConverterList");
        object[] paramValues = new object[] { JsonSerializerOptions.Default, jsonConverterList };
        var converterList = type!.GetConstructors()[0].Invoke(paramValues) as IList<JsonConverter>;
        typeof(JsonSerializerOptions).GetRuntimeFields().Single(f => f.Name == "_converters")
            .SetValue(JsonSerializerOptions.Default, converterList);
    }
}