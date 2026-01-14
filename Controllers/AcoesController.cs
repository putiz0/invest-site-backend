using Microsoft.AspNetCore.Mvc;
using InvestSite.API.Services;

namespace InvestSite.API.Controllers;

[ApiController]
[Route("api/acoes")]
public class AcoesController : ControllerBase
{
    private readonly AcaoService _service;

    public AcoesController(AcaoService service)
    {
        _service = service;
    }

    [HttpGet("preco-teto")]
    public IActionResult CalcularPrecoTeto(
        double dividendos,
        double lpa,
        double vpa)
    {
        return Ok(new
        {
            Bazin = _service.PrecoTetoBazin(dividendos),
            Graham = _service.PrecoTetoGraham(lpa, vpa)
        });
    }
}
