using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===================================================
// Controllers
// ===================================================
builder.Services.AddControllers();

// ===================================================
// CORS (Netlify / qualquer origem)
// ===================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ===================================================
// JWT Authentication
// ===================================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("INVESTSITE_SUPER_SECRETO_123"))
    };
});

var app = builder.Build();

// ===================================================
// HTTP PIPELINE
// ===================================================
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// ===================================================
// Controllers
// ===================================================
app.MapControllers();

// ===================================================
// Health Check
// ===================================================
app.MapGet("/", () => Results.Ok(new
{
    status = "Backend OK 🚀 Render funcionando"
}));

// ===================================================
// Porta dinâmica (Render)
// ===================================================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();
