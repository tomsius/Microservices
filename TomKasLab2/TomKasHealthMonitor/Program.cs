using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", "TomKasHealthMonitor")
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(builder.Configuration["Serilog:SeqServerUrl"])
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

Log.Information("Configuring web host ({ApplicationContext})...", "TomKasHealthMonitor");

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

//builder.Services.AddHealthChecksUI(options =>
//{
//    options.SetEvaluationTimeInSeconds(10);   
//    options.MaximumHistoryEntriesPerEndpoint(60);    
//    options.SetApiMaxActiveRequests(3); 
//})
//    .AddInMemoryStorage();

var app = builder.Build();

var pathBase = builder.Configuration["PATH_BASE"];
if (!string.IsNullOrEmpty(pathBase))
{
    app.UsePathBase(pathBase);
}

app.UseHealthChecksUI(config =>
{
    config.ResourcesPath = string.IsNullOrEmpty(pathBase) ? "/ui/resources" : $"{pathBase}/ui/resources";
    config.UIPath = "/hc-ui";
});

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
    {
        Predicate = r => r.Name.Contains("self")
    });
});

Log.Information("Starting web host ({ApplicationContext})...", "TomKasHealthMonitor");

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "TomKasHealthMonitor crashed!");
}
finally
{
    Log.CloseAndFlush();
}
