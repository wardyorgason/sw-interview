using System.Text.RegularExpressions;

namespace ServiceLayer.Extensions;

public static class StringExtensions
{
    // This was mostly written by ChatGPT
    public static int? ExtractSwapiId(this string? url)
    {
        if (url is null) return null;
        
        Regex regex = new Regex(@"https://swapi\.py4e\.com/api/\w+/(\d+)/", RegexOptions.Compiled);
        Match match = regex.Match(url);
        return match.Success ? int.Parse(match.Groups[1].Value) : (int?)null;
    }
}