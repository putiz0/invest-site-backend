using InvestSite.API.Models;
using InvestSite.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestSite.API.Controllers
{
    [ApiController]
    [Route("api/acoes")]
    public class AcoesController : ControllerBase
    {
        private readonly AcaoService _service;

        public AcoesController(AcaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(Acao acao)
        {
            return Ok(await _service.CreateAsync(acao));
        }
    }
}
