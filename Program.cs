using InvestSite.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===================================================
// MongoDB SETTINGS
// ===================================================
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings")
);

// MongoClient (Singleton)
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = builder.Configuration
        .GetSection("MongoSettings")
        .Get<MongoSettings>();

    if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
        throw new Exception("MongoSettings.ConnectionString não configurada");

    return new MongoClient(settings.ConnectionString);
});

// MongoDatabase (Scoped)
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = builder.Configuration
        .GetSection("MongoSettings")
        .Get<MongoSettings>();

    if (settings == null || string.IsNullOrWhiteSpace(settings.DatabaseName))
        throw new Exception("MongoSettings.DatabaseName não configurado");

    return client.GetDatabase(settings.DatabaseName);
});

// ===================================================
// Services
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
// Health Check
// ===================================================
app.MapGet("/", () => Results.Ok(new
{
    status = "Backend OK 🚀 Render funcionando"
}));

// ===================================================
// Teste Mongo
// ===================================================
app.MapGet("/api/test-mongo", async (IMongoDatabase db) =>
{
    var collections = await db.ListCollectionNamesAsync();
    var names = await collections.ToListAsync();
    return Results.Ok(new { ok = true, collections = names });
});

// ===================================================
// Porta dinâmica (Render)
// ===================================================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();

// ===================================================
// MongoSettings
// ===================================================
public class MongoSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}
