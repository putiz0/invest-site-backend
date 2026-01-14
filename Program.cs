using InvestSite.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===================================================
// MongoDB CONFIG
// ===================================================
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings")
);

// MongoClient → Singleton (CORRETO)
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = builder.Configuration
        .GetSection("MongoSettings")
        .Get<MongoSettings>();

    return new MongoClient(settings!.ConnectionString);
});

// IMongoDatabase → Scoped (CORRETO)
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = builder.Configuration
        .GetSection("MongoSettings")
        .Get<MongoSettings>();

    return client.GetDatabase(settings!.DatabaseName);
});

// ===================================================
// SERVICES (Scoped)
// ===================================================
builder.Services.AddScoped<FiiService>();
builder.Services.AddScoped<AcaoService>();

// ===================================================
// Controllers
// ===================================================
builder.Services.AddControllers();

// ===================================================
// Swagger (APENAS DEV)
// ===================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===================================================
// CORS (Netlify)
// ===================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ===================================================
// JWT Auth
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ===================================================
// Health Check
// ===================================================
app.MapGet("/", () => Results.Ok(new
{
    status = "InvestSite API rodando 🚀"
}));

// ===================================================
// Porta dinâmica (Render)
// ===================================================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();

// ===================================================
// Mongo Settings
// ===================================================
public class MongoSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}
