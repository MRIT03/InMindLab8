using Asp.Versioning;
using InMindLab8.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;


[ApiVersion(1.0)]
[ApiController]
[Route("api/v{apiVersion:apiVersion}/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IStringLocalizer<SharedResource> localizer, ILogger<HomeController> logger)
    {
        _localizer = localizer;
        _logger = logger;
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        // Retrieve a localized string using a key defined in your resource file
        var message = _localizer["Hello"];
        Console.WriteLine("message: " +message);
        return Ok(new { Message = message });
    }
    
    [HttpGet("logtest")]
    public IActionResult LogTest()
    {
        _logger.LogInformation("This is an informational message.");
        _logger.LogWarning("This is a warning!");
        _logger.LogError("This is an error message!");
        
        return Ok("Logs have been recorded.");
    }
}