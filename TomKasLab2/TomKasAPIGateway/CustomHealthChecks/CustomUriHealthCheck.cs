using Serilog;
using TomKasAPIGateway.Models;

namespace TomKasAPIGateway.CustomHealthChecks;

public class CustomUriHealthCheck
{
    public string Uri { get; }

    public CustomUriHealthCheck(string uri)
    {
        Uri = uri ?? throw new ArgumentNullException(nameof(uri));
    }

    public async Task<CustomHealthResult> CheckHealthAsync()
    {
        Log.Information("Checking health of {Uri}...", Uri);

        HttpClient client = new();

        try
        {
            using HttpResponseMessage response = await client.GetAsync(Uri);

            if (((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299) == false)
            {
                Log.Warning("{Uri} is not responding with code in 200...299 range, the current status is {StatusCode}.", Uri, response.StatusCode);
                return new CustomHealthResult("Unhealthy", $"{Uri} is not responding with code in 200...299 range, the current status is {response.StatusCode}.");
            }

            Log.Information("{Uri} is healthy.", Uri);
            return new CustomHealthResult("Healthy", "");
        }
        catch (Exception)
        {
            Log.Warning("{Uri} cannot be reached.", Uri);
            return new CustomHealthResult("Unhealthy", $"{Uri} cannot be reached.");
        }
    }
}
