using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FraudSys.Models;

namespace FraudSys.Controllers;

public class GestaoLimitesController : Controller
{
    private readonly ILogger<GestaoLimitesController> _logger;

    public GestaoLimitesController(ILogger<GestaoLimitesController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CadastrarLimite() 
    {
        return View();
    }

    public IActionResult NovoCadastro(Limite model) 
    {
        return RedirectToAction("Index");
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
