using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Text.Json.Serialization;
using TomKasStudentsAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TomKasStudentsAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TomKasStudentsAPIContext") ?? throw new InvalidOperationException("Connection string 'TomKasStudentsAPIContext' not found.")));

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", "TomKasStudentsAPI")
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(builder.Configuration["Serilog:SeqServerUrl"])
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

Log.Information("Configuring web host ({ApplicationContext})...", "TomKasStudentsAPI");

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddSqlServer(builder.Configuration.GetConnectionString("TomKasStudentsAPIContext"), name: "TomKasStudentsDB-Check", tags: new string[] { "tomkasstudentsdb" });

var app = builder.Build();

Log.Information("Seeding database for ({ApplicationContext})...", "TomKasStudentsAPI");

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

Log.Information("Starting web host ({ApplicationContext})...", "TomKasStudentsAPI");

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "TomKasStudentsAPI crashed!");
}
finally
{
    Log.CloseAndFlush();
}
