using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FraudSys.Models;

namespace FraudSys.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GestaoLimites() 
    {
        return View();
    }

    public IActionResult CadastrarLimite() 
    {
        return View();
    }

    public IActionResult BuscarLimite() 
    {
        return View();
    }

    public IActionResult AtualizarLimite() 
    {
        return View();
    }

    public IActionResult RemoverLimite() 
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
