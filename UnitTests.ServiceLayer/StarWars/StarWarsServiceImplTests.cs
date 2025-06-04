using Dao.External;
using Dtos.External.StarWarsApi;
using Dtos.StarWars;
using Moq;
using NUnit.Framework;
using ServiceLayer.StarWars._Impl;

namespace UnitTests.ServiceLayer.StarWars;

[TestFixture]
public class StarWarsServiceImplTests
{
    private StarWarsServiceImpl _service;

    private Mock<StarWarsApiDao> _apiDaoMock;

    [SetUp]
    public void SetUp()
    {
        _apiDaoMock = new(MockBehavior.Strict);
        _service = new(_apiDaoMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _apiDaoMock.Verify();
    }
    
    // Tests mostly written by ChatGPT
    [Test]
    public async Task GetAllFilms_ReturnsListOfFilms_WhenApiReturnsFilms()
    {
        // Arrange
        SwApiListingResult<SwApiFilm> mockFilms = new()
        {
            Count = 2,
            Results = new List<SwApiFilm>
            {
                new() { Title = "A New Hope", EpisodeId = 4 },
                new() { Title = "The Empire Strikes Back", EpisodeId = 5 }
            }
        };

        _apiDaoMock.Setup(dao => dao.ListAllFilms())
            .ReturnsAsync(mockFilms);

        // Act
        List<SwApiFilm> result = await _service.GetAllFilms();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Title, Is.EqualTo("A New Hope"));
        Assert.That(result[1].Title, Is.EqualTo("The Empire Strikes Back"));
    }

    [Test]
    public async Task GetAllFilms_ReturnsEmptyList_WhenApiReturnsNull()
    {
        // Arrange
        _apiDaoMock.Setup(dao => dao.ListAllFilms())
            .ReturnsAsync((SwApiListingResult<SwApiFilm>)null!);

        // Act
        List<SwApiFilm> result = await _service.GetAllFilms();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetFilmBreakdownChartData_ReturnsChartData_WhenApiReturnsFilms()
    {
        // Arrange
        SwApiListingResult<SwApiFilm> mockFilms = new()
        {
            Count = 2,
            Results = new List<SwApiFilm>
            {
                new()
                { 
                    Title = "A New Hope", 
                    EpisodeId = 4,
                    StarshipUrls = new List<SwApiUrl> { new(){Url="url1"}, new(){Url="url2"} },
                    CharacterUrls = new List<SwApiUrl> { new(){Url="url1"}, new(){Url="url2"}, new(){Url="url3"} },
                    PlanetUrls = new List<SwApiUrl> { new(){Url="url1"} }
                },
                new()
                { 
                    Title = "The Empire Strikes Back", 
                    EpisodeId = 5,
                    StarshipUrls = new List<SwApiUrl> { new(){Url="url1"}, new(){Url="url2"}, new(){Url="url3"} },
                    CharacterUrls = new List<SwApiUrl> { new(){Url="url1"}, new(){Url="url2"} },
                    PlanetUrls = new List<SwApiUrl> { new(){Url="url1"}, new(){Url="url2"} }
                }
            }
        };

        _apiDaoMock.Setup(dao => dao.ListAllFilms())
            .ReturnsAsync(mockFilms);

        // Act
        FilmBreakdownChartData result = await _service.GetFilmBreakdownChartData();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Data.Count, Is.EqualTo(2));
        
        // Check first film data
        (SwApiFilm Film, FilmBreakdownSpecifics Specifics) firstFilmData = result.Data[0];
        Assert.That(firstFilmData.Film.Title, Is.EqualTo("A New Hope"));
        Assert.That(firstFilmData.Specifics.Starships, Is.EqualTo(2));
        Assert.That(firstFilmData.Specifics.People, Is.EqualTo(3));
        Assert.That(firstFilmData.Specifics.Planets, Is.EqualTo(1));
        
        // Check second film data
        (SwApiFilm Film, FilmBreakdownSpecifics Specifics) secondFilmData = result.Data[1];
        Assert.That(secondFilmData.Film.Title, Is.EqualTo("The Empire Strikes Back"));
        Assert.That(secondFilmData.Specifics.Starships, Is.EqualTo(3));
        Assert.That(secondFilmData.Specifics.People, Is.EqualTo(2));
        Assert.That(secondFilmData.Specifics.Planets, Is.EqualTo(2));
    }

    [Test]
    public void GetFilmBreakdownChartData_ThrowsException_WhenApiReturnsNull()
    {
        // Arrange
        _apiDaoMock.Setup(dao => dao.ListAllFilms())
            .ReturnsAsync((SwApiListingResult<SwApiFilm>)null!);

        // Act & Assert
        NullReferenceException? exception = Assert.ThrowsAsync<NullReferenceException>(
            async () => await _service.GetFilmBreakdownChartData());
        
        Assert.That(exception?.Message, Is.EqualTo("Unable to retrieve films from API"));
    }
    
    [Test]
    public async Task SearchStarships_ReturnsAllStarships_WhenSearchStringIsNull()
    {
        // Arrange
        SwApiListingResult<SwApiStarship> mockStarships = new SwApiListingResult<SwApiStarship>
        {
            Count = 2,
            Results = new List<SwApiStarship>
            {
                new() { Name = "X-Wing", Crew = "1", CargoCapacity = "110" },
                new() { Name = "Millennium Falcon", Crew = "4", CargoCapacity = "100000" }
            }
        };
        
        _apiDaoMock.Setup(dao => dao.ListAllStarships())
            .ReturnsAsync(mockStarships);
        
        // Act
        List<SwApiStarship> result = await _service.SearchStarships(null);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("X-Wing"));
        Assert.That(result[1].Name, Is.EqualTo("Millennium Falcon"));
    }

    [Test]
    public async Task SearchStarships_ReturnsAllStarships_WhenSearchStringIsEmpty()
    {
        // Arrange
        SwApiListingResult<SwApiStarship> mockStarships = new SwApiListingResult<SwApiStarship>
        {
            Count = 2,
            Results = new List<SwApiStarship>
            {
                new() { Name = "X-Wing", Crew = "1", CargoCapacity = "110" },
                new() { Name = "Millennium Falcon", Crew = "4", CargoCapacity = "100000" }
            }
        };
        
        _apiDaoMock.Setup(dao => dao.ListAllStarships())
            .ReturnsAsync(mockStarships);
        
        // Act
        List<SwApiStarship> result = await _service.SearchStarships("");
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task SearchStarships_ReturnsFilteredStarships_WhenSearchingByName()
    {
        // Arrange
        SwApiListingResult<SwApiStarship> mockStarships = new SwApiListingResult<SwApiStarship>
        {
            Count = 3,
            Results = new List<SwApiStarship>
            {
                new() { Name = "X-Wing", Crew = "1", CargoCapacity = "110" },
                new() { Name = "Millennium Falcon", Crew = "4", CargoCapacity = "100000" },
                new() { Name = "Star Destroyer", Crew = "47,060", CargoCapacity = "36000000" }
            }
        };
        
        _apiDaoMock.Setup(dao => dao.ListAllStarships())
            .ReturnsAsync(mockStarships);
        
        // Act
        List<SwApiStarship> result = await _service.SearchStarships("star");
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Star Destroyer"));
    }

    [Test]
    public async Task SearchStarships_ReturnsFilteredStarships_WhenSearchingByCrew()
    {
        // Arrange
        SwApiListingResult<SwApiStarship> mockStarships = new SwApiListingResult<SwApiStarship>
        {
            Count = 3,
            Results = new List<SwApiStarship>
            {
                new() { Name = "X-Wing", Crew = "1", CargoCapacity = "110" },
                new() { Name = "Millennium Falcon", Crew = "4", CargoCapacity = "100000" },
                new() { Name = "Star Destroyer", Crew = "47,060", CargoCapacity = "36000000" }
            }
        };
        
        _apiDaoMock.Setup(dao => dao.ListAllStarships())
            .ReturnsAsync(mockStarships);
        
        // Act
        List<SwApiStarship> result = await _service.SearchStarships("47");
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Star Destroyer"));
    }

    [Test]
    public async Task SearchStarships_ReturnsFilteredStarships_WhenSearchingByCargoCapacity()
    {
        // Arrange
        SwApiListingResult<SwApiStarship> mockStarships = new SwApiListingResult<SwApiStarship>
        {
            Count = 3,
            Results = new List<SwApiStarship>
            {
                new() { Name = "X-Wing", Crew = "1", CargoCapacity = "110" },
                new() { Name = "Millennium Falcon", Crew = "4", CargoCapacity = "100000" },
                new() { Name = "Star Destroyer", Crew = "47,060", CargoCapacity = "36000000" }
            }
        };
        
        _apiDaoMock.Setup(dao => dao.ListAllStarships())
            .ReturnsAsync(mockStarships);
        
        // Act
        List<SwApiStarship> result = await _service.SearchStarships("3600");
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Star Destroyer"));
    }

    [Test]
    public async Task SearchStarships_IsCaseInsensitive()
    {
        // Arrange
        SwApiListingResult<SwApiStarship> mockStarships = new SwApiListingResult<SwApiStarship>
        {
            Count = 2,
            Results = new List<SwApiStarship>
            {
                new() { Name = "X-Wing", Crew = "1", CargoCapacity = "110" },
                new() { Name = "Millennium Falcon", Crew = "4", CargoCapacity = "100000" }
            }
        };
        
        _apiDaoMock.Setup(dao => dao.ListAllStarships())
            .ReturnsAsync(mockStarships);
        
        // Act
        List<SwApiStarship> result = await _service.SearchStarships("FALCON");
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Millennium Falcon"));
    }

    [Test]
    public void SearchStarships_ThrowsException_WhenApiReturnsNull()
    {
        // Arrange
        _apiDaoMock.Setup(dao => dao.ListAllStarships())
            .ReturnsAsync((SwApiListingResult<SwApiStarship>)null!);
        
        // Act & Assert
        NullReferenceException? exception = Assert.ThrowsAsync<NullReferenceException>(
            async () => await _service.SearchStarships("test"));
            
        Assert.That(exception?.Message, Is.EqualTo("Unable to retrieve starships"));
    }
}