using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FraudSys.Models;
using FraudSys.Repositories;

namespace FraudSys.Controllers;

public class GestaoLimitesController : Controller
{
    private readonly ILogger<GestaoLimitesController> _logger;
    private readonly ILimiteRepository _repository;

    public GestaoLimitesController(ILogger<GestaoLimitesController> logger, ILimiteRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public IActionResult Index()
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

    [HttpPost]
    public async Task<IActionResult> Cadastrar(Limite limite) 
    {
        await _repository.Cadastrar(limite);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Buscar(Limite limite) {
        var res = await _repository.Buscar(limite.agencia, limite.conta);

        if (res != null) {
            return View("BuscarLimite", res);
        }
        else {
            ViewBag.Message = "Nenhum resultado encontrado.";
            return View("BuscarLimite");
        }
    
    }

    [HttpPost]
    public async Task<IActionResult> Atualizar(Limite limite) {
        var meuLimite = await _repository.Buscar(limite.agencia, limite.conta);

        if (meuLimite == null) {
            ViewBag.Message = "Registro não encontrado para atualização.";
            return View("AtualizarLimite");
        }

        meuLimite.limitePix = limite.limitePix;
        await _repository.Atualizar(meuLimite);

        return RedirectToAction("Index");
    }

    [HttpDelete]
    public async Task<IActionResult> Deletar(string agencia, string conta) {
        await _repository.Remover(agencia, conta);
        return Ok();
    }
}
