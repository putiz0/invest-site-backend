using Microsoft.AspNetCore.Mvc;

namespace InvestSite.API.Controllers;

[ApiController]
[Route("api/juros")]
public class JurosController : ControllerBase
{
    [HttpGet("compostos")]
    public IActionResult Calcular(
        double capitalInicial,
        double aporteMensal,
        double taxaMensal,
        int meses)
    {
        double montante = capitalInicial;

        for (int i = 0; i < meses; i++)
        {
            montante = montante * (1 + taxaMensal / 100) + aporteMensal;
        }

        return Ok(new
        {
            capitalInicial,
            aporteMensal,
            taxaMensal,
            meses,
            montanteFinal = Math.Round(montante, 2)
        });
    }
}
