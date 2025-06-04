using Dtos.External.StarWarsApi;

namespace ServiceLayer.StarWars._Impl;

public static class StarWarsServiceExtensions
{
    public static IEnumerable<SwApiStarship> WhereMatchesIntSearch(this IEnumerable<SwApiStarship> searchSpace, int search)
    {
        return searchSpace.Where(s =>
            s.Name.ToLower().Contains(search.ToString()) ||
            SwApiNumberMatches(s.Crew, search) ||
            SwApiNumberMatches(s.CargoCapacity, search)
        );
    }

    public static IEnumerable<SwApiStarship> WhereMatchesStringSearch(this IEnumerable<SwApiStarship> searchSpace,
        string search)
    {
        return searchSpace.Where(s => s.Name.ToLower().Contains(search));
    }

    private static bool SwApiNumberMatches(SwApiNumber number, int searchNumber)
    {
        if (number is SwApiSingleNumber singleNumber)
        {
            return singleNumber.Value == searchNumber;
        }

        if (number is SwApiNumberRange numberRange)
        {
            return searchNumber >= numberRange.Start && searchNumber <= numberRange.End;
        }

        throw new NotImplementedException($"Comparison not implemented for type {number.GetType().Name}");
    }
    
}