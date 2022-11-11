namespace TomKasAPIGateway.Models;

public class CustomHealthResult
{
	public string Status { get; }
	public string Exception { get; }

	public CustomHealthResult(string status, string exception)
	{
		Status = status;
		Exception = exception;
	}
}
