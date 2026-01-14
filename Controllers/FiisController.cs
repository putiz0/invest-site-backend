using InvestSite.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestSite.API.Controllers;

[ApiController]
[Route("api/fiis")]
public class FiisController : ControllerBase
{
    private readonly FiiService _service;

    public FiisController(FiiService service)
    {
        _service = service;
    }

    [HttpGet("ranking")]
    public IActionResult GetRanking()
    {
        return Ok(_service.GetRanking());
    }
}
