namespace Dtos.External.StarWarsApi;

public abstract record SwApiNumber();

public record SwApiSingleNumber(long Value): SwApiNumber;
public record SwApiNumberRange(long Start, long End): SwApiNumber;