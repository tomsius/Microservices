using Microsoft.AspNetCore.Mvc;

namespace TomKasHealthMonitor.Controllers;

public class HomeController : Controller
{
    private IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var basePath = _configuration["PATH_BASE"];
        return Redirect($"{basePath}/hc-ui");
    }

    public IActionResult Error()
    {
        return View();
    }
}
