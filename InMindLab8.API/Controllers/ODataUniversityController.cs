using Asp.Versioning;
using InMindLab8.Application.Queries;
using InMindLab8.Application.ViewModels;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Caching.Memory;

namespace InMindLab8.API.Controllers;
[ApiVersion(2.0)]
[ApiController]
[Route ("api/v{version:apiVersion}/odata/")]

public class ODataUniversityController : ODataController
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _memoryCache;

    public ODataUniversityController(IMediator mediator, IMemoryCache memoryCache)
    {
        _mediator = mediator;
        _memoryCache = memoryCache;
    }

    [HttpGet("[action]")]
    [EnableQuery]
    public async Task<IActionResult> GetStudents()
    {
        return Ok(await _mediator.Send(new RetrieveStudentsQuery()));
    }
    
    [HttpGet("[action]")]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Filter)]
    public async Task<IActionResult> GetTeachers()
    {
        return Ok(await _mediator.Send(new RetrieveTeachersQuery()));
    }
    
    [HttpGet("[action]")]
    [EnableQuery]
    public async Task<IActionResult> GetCourses()
    {
        return Ok(await _mediator.Send(new RetrieveCoursesQuery()));
    }

    [HttpGet("[action]")]
    [EnableQuery]
    public async Task<IActionResult> GetAdmins()
    {
        string cache_key = "admins";

        if (_memoryCache.TryGetValue(cache_key, out IActionResult value))
        {
            return value;
        }

        var admins = await _mediator.Send(new RetrieveAdminsQuery());

        if (admins.Count > 0)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
            };
            _memoryCache.Set(cache_key, admins, cacheEntryOptions);
        }
        return Ok(admins);
    }
}