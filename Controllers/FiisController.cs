using InvestSite.API.Models;
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

    // 🔹 GET /api/fiis
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var fiis = await _service.GetAllAsync();
        return Ok(fiis);
    }

    // 🔹 POST /api/fiis
    [HttpPost]
    public async Task<IActionResult> Create(Fii fii)
    {
        var created = await _service.CreateAsync(fii);
        return Ok(created);
    }

    // 🔹 GET /api/fiis/ranking
    [HttpGet("ranking")]
    public async Task<IActionResult> Ranking()
    {
        var ranking = await _service.GetRankingAsync();
        return Ok(ranking);
    }
}
