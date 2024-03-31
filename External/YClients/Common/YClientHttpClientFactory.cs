using System.Net.Http.Headers;

namespace LocaSubs.External.YClients.Common;

public class YClientHttpClientFactory
{
    private const string MediaType = "application/vnd.api.v2+json";
    private const string AuthenticationScheme = "Bearer";
    private const string Token = "pydtbd2ssznkpxuf9wra";

    public HttpClient CreateClient()
    {
        HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(MediaType));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthenticationScheme, Token);
        return client;
    }
}
