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

    [HttpGet]
    public async Task<IActionResult> Buscar(string agencia, string conta) {
        var lista = await _repository.Buscar(agencia, conta);
        Console.WriteLine(lista);
        return Ok(lista);
    }

    [HttpPut]
    public async Task<IActionResult> Atualizar(Limite limite) {
        await _repository.Atualizar(limite);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Deletar(string agencia, string conta) {
        await _repository.Remover(agencia, conta);
        return Ok();
    }
}
