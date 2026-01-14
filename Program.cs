using InvestSite.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===================================================
// MongoSettings (EXISTE PARA NÃO QUEBRAR COMPILAÇÃO)
// ===================================================
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings")
);

// ===================================================
// Services
// ⚠️ Registrados, mas NÃO usados ainda
// ===================================================
builder.Services.AddScoped<MongoService>();
builder.Services.AddScoped<FiiService>();
builder.Services.AddScoped<AcaoService>();

// ===================================================
// Controllers
// ===================================================
builder.Services.AddControllers();

// ===================================================
// CORS
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
// PIPELINE
// ===================================================
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ===================================================
// Health check
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

// ===================================================
// MongoSettings (NECESSÁRIO PARA MongoService.cs)
// ===================================================
public class MongoSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}
