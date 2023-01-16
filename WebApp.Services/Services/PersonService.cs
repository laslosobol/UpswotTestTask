using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApp.Services.Data;

namespace WebApp.Services.Services;

public class PersonService
{
    private readonly HttpClient _httpClient;

    public PersonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PersonResponse?> GetPerson(string personName)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PersonResponse>($"https://rickandmortyapi.com/api/character/?name={personName}", new JsonSerializerOptions()
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