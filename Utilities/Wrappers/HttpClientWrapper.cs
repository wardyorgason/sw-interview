namespace Utilities.Wrappers;

public interface HttpClientWrapper
{
    Task<HttpResponseMessage> GetAsync(string url);
    Task<string> GetStringAsync(string uri);
    Task<T> GetFromJsonAsync<T>(string? requestUri, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T value);
    Task<string> GetStringAsyncWithBasicAuth(string username, string password, string url);
    Task<string> PostJsonAsyncWithBasicAuth(string username, string password, string url, string jsonString);
    Task<string> PutJsonAsyncWithBasicAuth(string username, string password, string url, string jsonString);

    Task<HttpResponseMessage> PostAsync(string url, HttpContent content,
        CancellationToken cancellationToken = default);

    Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage);
    Task<byte[]> GetByteArrayAsync(string uri);
}
