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
        HttpClient client = new();

        try
        {
            using HttpResponseMessage response = await client.GetAsync(Uri);

            if (((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299) == false)
            {
                return new CustomHealthResult("Unhealthy", $"{Uri} is not responding with code in 200...299 range, the current status is {response.StatusCode}.");
            }

            return new CustomHealthResult("Healthy", "");
        }
        catch (Exception)
        {
            return new CustomHealthResult("Unhealthy", $"{Uri} cannot be reached.");
        }
        
    }
}
