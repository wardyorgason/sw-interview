using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Utilities.Wrappers._Impl;

public class HttpClientWrapperImpl : HttpClientWrapper
{
    private (DateTime IssueDate, HttpClient Client)? _currentClient;
    private readonly HttpClientHandler _handler;
    private HttpClient HttpClient
    {
        get
        {
            if(_currentClient == null || _currentClient.Value.IssueDate.AddHours(30) < DateTime.Now)
            {
                _currentClient?.Client.Dispose();
                _currentClient = (DateTime.Now, new HttpClient());
            }

            return _currentClient.Value.Client;
        }
    }


     /// <summary>
     /// Constructor for when we want to store session information (not for singletons)
     /// </summary>
     /// <param name="handler"></param>
     /// <param name="logger"></param>
    public HttpClientWrapperImpl(HttpClientHandler handler)
    {
        _handler = handler;
    }
    public async Task<string> GetStringAsync(string requestUri)
    {
        return await HttpClient.GetStringAsync(requestUri).ConfigureAwait(false);
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        return await HttpClient.GetAsync(url).ConfigureAwait(false);
    }

    public async Task<T> GetFromJsonAsync<T>(string? requestUri, CancellationToken cancellationToken = default)
    {
        return await HttpClient.GetFromJsonAsync<T>(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T value)
    {
        return await HttpClient.PostAsJsonAsync(url, value).ConfigureAwait(false);
    }

    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken cancellationToken = default)
    {
        return await HttpClient.PostAsync(url, content, cancellationToken).ConfigureAwait(false);
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage)
    {
        return await HttpClient.SendAsync(requestMessage).ConfigureAwait(false);
    }

    public async Task<byte[]> GetByteArrayAsync(string uri)
    {
        return await HttpClient.GetByteArrayAsync(uri).ConfigureAwait(false);
    }

    public async Task<string> GetStringAsyncWithBasicAuth(string username, string password, string url)
    {
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url);
        Encoding encoding = Encoding.ASCII;

        message.Headers.Authorization = new AuthenticationHeaderValue(
            scheme: "Basic",
            parameter: Convert.ToBase64String(encoding.GetBytes(username + ":" + password))
        );

        HttpResponseMessage res = await HttpClient.SendAsync(message).ConfigureAwait(false);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadAsStringAsync().ConfigureAwait(false);
    }
    public async Task<string> PostJsonAsyncWithBasicAuth(string username, string password, string url, string jsonString)
    {
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, url);
        Encoding encoding = Encoding.ASCII;

        message.Headers.Authorization = new AuthenticationHeaderValue(
            scheme: "Basic",
            parameter: Convert.ToBase64String(encoding.GetBytes(username + ":" + password))
        );
        message.Content = new StringContent(jsonString);

        HttpResponseMessage res = await HttpClient.SendAsync(message).ConfigureAwait(false);
        res.EnsureSuccessStatusCode();

        return await res.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    public async Task<string> PutJsonAsyncWithBasicAuth(string username, string password, string url, string jsonString)
    {
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, url);
        Encoding encoding = Encoding.ASCII;

        message.Headers.Authorization = new AuthenticationHeaderValue(
            scheme: "Basic",
            parameter: Convert.ToBase64String(encoding.GetBytes(username + ":" + password))
        );
        message.Content = new StringContent(jsonString);

        HttpResponseMessage res = await HttpClient.SendAsync(message).ConfigureAwait(false);
        res.EnsureSuccessStatusCode();

        return await res.Content.ReadAsStringAsync().ConfigureAwait(false);
    }
    
}
