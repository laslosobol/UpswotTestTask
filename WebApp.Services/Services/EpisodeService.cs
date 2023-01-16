using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApp.Services.Data;

namespace WebApp.Services.Services;

public class EpisodeService
{
    private readonly HttpClient _httpClient;


    public EpisodeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<EpisodeResponse?> GetEpisode(string episodeName)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<EpisodeResponse>(
                $"https://rickandmortyapi.com/api/episode/?name={episodeName}", new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
        }
        catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }
}