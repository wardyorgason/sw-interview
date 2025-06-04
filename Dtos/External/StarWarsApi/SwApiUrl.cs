using Dtos.Extensions;

namespace Dtos.External.StarWarsApi;

public class SwApiUrl
{
    public string Url { get; set; }
    public int Id => Url.ExtractSwApiId() ?? -1;
}