using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InvestSite.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private const string SECRET = "INVESTSITE_SUPER_SECRETO_123";

    [HttpPost("login")]
    public IActionResult Login(string email, string plano)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim("plano", plano)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(SECRET));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256)
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}
