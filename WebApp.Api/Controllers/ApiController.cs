using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Data;
using WebApp.Services.Services;

namespace WebApp.Api.Controllers;

[ApiController]
[Route("/api/v1")]
[ResponseCache(Duration = 30 * 60)]
public class PersonController : ControllerBase
{
    private readonly PersonService _personService;
    private readonly EpisodeService _episodeService;

    public PersonController(EpisodeService episodeService, PersonService personService)
    {
        _episodeService = episodeService;
        _personService = personService;
    }

    [HttpPost]
    [Route("check-person")]
    public async Task<ActionResult<bool>> CheckPerson(string personName, string episodeName)
    {
        var personResponse = await _personService.GetPerson(personName);
        var episodeResponse = await _episodeService.GetEpisode(episodeName);
        
        if (personResponse is null || personResponse.Results.Length == 0)
        {
            return NotFound("Person not found");
        }

        if (episodeResponse is null || episodeResponse.Results.Length == 0)
        {
            return NotFound("Episode not found");
        }

        return Ok(personResponse.Results[0].Episode.Contains(episodeResponse.Results[0].Url));
    }

    [HttpGet]
    [Route("person")]
    public async Task<ActionResult<GetPersonResult>> GetPerson(string personName)
    {
        var personInfo = await _personService.GetPerson(personName);

        if (personInfo is null || personInfo.Results.Length == 0)
        {
            return NotFound("Person not found");
        }

        return Ok(new GetPersonResult()
        {
            Name = personInfo.Results[0].Name,
            Status = personInfo.Results[0].Status,
            Species = personInfo.Results[0].Species,
            Type = personInfo.Results[0].Type,
            Gender = personInfo.Results[0].Gender,
            Origin = personInfo.Results[0].Origin
        });
    }
}