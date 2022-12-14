using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Text.Json.Serialization;
using TomKasCoursesAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TomKasCoursesAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TomKasCoursesAPIContext") ?? throw new InvalidOperationException("Connection string 'TomKasCoursesAPIContext' not found.")));

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", "TomKasCoursesAPI")
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(builder.Configuration["Serilog:SeqServerUrl"])
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

Log.Information("Configuring web host ({ApplicationContext})...", "TomKasCoursesAPI");

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddSqlServer(builder.Configuration.GetConnectionString("TomKasCoursesAPIContext"), name: "TomKasCoursesDB-Check", tags: new string[] { "tomkascoursesdb" });

var app = builder.Build();

Log.Information("Seeding database for ({ApplicationContext})...", "TomKasCoursesAPI");

using IServiceScope scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;
SeedData.Initialize(services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/hc", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
    {
        Predicate = r => r.Name.Contains("self")
    });
});

Log.Information("Starting web host ({ApplicationContext})...", "TomKasCoursesAPI");

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "TomKasCoursesAPI crashed!");
}
finally
{
    Log.CloseAndFlush();
}
