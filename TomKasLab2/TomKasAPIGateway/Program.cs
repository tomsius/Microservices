using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using TomKasAPIGateway.CustomHealthChecks;
using TomKasAPIGateway.Models;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", "TomKasAPIGateway")
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(builder.Configuration["Serilog:SeqServerUrl"])
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

Log.Information("Configuring web host ({ApplicationContext})...", "TomKasAPIGateway");

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("ocelot.json");

builder.Services.AddOcelot();

var app = builder.Build();

var conf = new OcelotPipelineConfiguration()
{
    PreErrorResponderMiddleware = async (ctx, next) =>
    {
        if (ctx.Request.Path.Equals(new PathString("/hc")))
        {
            DateTimeOffset selfStart = DateTimeOffset.Now;
            DateTimeOffset selfEnd = DateTimeOffset.Now;

            //Log.Information("Checking health of {TomKasStudentsUrlHC}...", builder.Configuration["TomKasStudentsUrlHC"]);

            DateTimeOffset db1Start = DateTimeOffset.Now;
            CustomHealthResult db1Result = await new CustomUriHealthCheck(builder.Configuration["TomKasStudentsUrlHC"]).CheckHealthAsync();
            DateTimeOffset db1End = DateTimeOffset.Now;

            string db1Json = "";
            if (db1Result.Status == "Healthy")
            {
                db1Json = $"\"TomKasStudentsAPI-Check\": {{\"data\": {{}},\"duration\": \"{db1End - db1Start}\",\"status\": \"{db1Result.Status}\",\"tags\": [\"tomkasstudentsapi\"]}}";
            }
            else
            {
                db1Json = $"\"TomKasStudentsAPI-Check\": {{\"data\": {{}},\"description\": \"{db1Result.Exception}\",\"duration\": \"{db1End - db1Start}\",\"exception\": \"{db1Result.Exception}\",\"status\": \"{db1Result.Status}\",\"tags\": [\"tomkasstudentsapi\"]}}";
            }

            //Log.Information("Checking health of {TomKasCoursesUrlHC}...", builder.Configuration["TomKasCoursesUrlHC"]);

            DateTimeOffset db2Start = DateTimeOffset.Now;
            CustomHealthResult db2Result = await new CustomUriHealthCheck(builder.Configuration["TomKasCoursesUrlHC"]).CheckHealthAsync();
            DateTimeOffset db2End = DateTimeOffset.Now;

            string db2Json = "";
            if (db2Result.Status == "Healthy")
            {
                db2Json = $"\"TomKasCoursesAPI-Check\": {{\"data\": {{}},\"duration\": \"{db2End - db2Start}\",\"status\": \"{db2Result.Status}\",\"tags\": [\"tomkascoursesapi\"]}}";
            }
            else
            {
                db2Json = $"\"TomKasCoursesAPI-Check\": {{\"data\": {{}},\"description\": \"{db2Result.Exception}\",\"duration\": \"{db2End - db2Start}\",\"exception\": \"{db2Result.Exception}\",\"status\": \"{db2Result.Status}\",\"tags\": [\"tomkascoursesapi\"]}}";
            }

            string json = $"{{\"status\": \"Healthy\",\"totalDuration\": \"{selfEnd - selfStart}\",\"entries\": {{{db1Json},{db2Json}}}}}";
            await ctx.Response.WriteAsync(json);
        }
        else if (ctx.Request.Path.Equals(new PathString("/liveness")))
        {
            await ctx.Response.WriteAsync("Healthy");
        }
        else
        {
            await next.Invoke();
        }
    }
};

app.UseOcelot(conf).Wait();

Log.Information("Starting web host ({ApplicationContext})...", "TomKasAPIGateway");

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "TomKasAPIGateway crashed!");
}
finally
{
    Log.CloseAndFlush();
}
