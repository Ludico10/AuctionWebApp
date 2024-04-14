using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Extensions;
using AuctionWebApp.Server.Interfaces;
using AuctionWebApp.Server.Services;
using Hangfire;
using Hangfire.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var authOptions = new AuthOptions(
        Issuer: "auction-auth",
        Audience: "auction-app",
        Key: "superSecretKey@34512345678912345",
        Lifetime: TimeSpan.FromMinutes(10)
        );

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AuctionDb");
builder.Services.AddDbContext<MySqlContext>(options => options.UseMySql(connectionString, ServerVersion.Parse("8.0.32-mysql")));

builder.Services.AddHangfire(h => h.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                   .UseSimpleAssemblyNameTypeSerializer()
                                   .UseRecommendedSerializerSettings()
                                    .UseStorage(
                                                new MySqlStorage(
                                                connectionString,
                                                new MySqlStorageOptions
                                                {
                                                    QueuePollInterval = TimeSpan.FromSeconds(10),
                                                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                                                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                                                    PrepareSchemaIfNecessary = true,
                                                    DashboardJobListLimit = 25000,
                                                    TransactionTimeout = TimeSpan.FromMinutes(1),
                                                    TablesPrefix = "Hangfire",
                                                })));

builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<ITokenService, TokenService>(_ => new TokenService(authOptions));
builder.Services.AddScoped<ISimulationService, SimulationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme"

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(builder.Configuration);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard("/dashboard");
}

//app.UseHttpsRedirection();
app.UseCors("SusloPolicy");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

RecurringJob.AddOrUpdate("AuctionsClosing", (IAuctionService service) => service.AuctionsClosing(), Cron.Minutely);

app.Run();
