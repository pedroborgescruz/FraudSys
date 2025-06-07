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

    public async Task<IActionResult> Index() {
        var todosOsLimites = await _repository.BuscarTodos();
        return View(todosOsLimites);
    }
    public IActionResult CadastrarLimite() {
        return View();
    }
    public IActionResult BuscarLimite() {
        return View();
    }
    public IActionResult AtualizarLimite() {
        return View();
    }
    public IActionResult RemoverLimite() {
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

    [HttpPost]
    public async Task<IActionResult> Deletar(string agencia, string conta) {
        var meuLimite = await _repository.Buscar(agencia, conta);

        if (meuLimite == null) {
            ViewBag.Message = "Registro não encontrado para deleção.";
            return View("RemoverLimite");
        }

        await _repository.Remover(agencia, conta);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("api/limites/processar-pix")]
    public async Task<IActionResult> ProcessarPix([FromBody] TransacaoPix transacao) {
        // Validar dados da transação.
        if (transacao == null || string.IsNullOrEmpty(transacao.conta) || string.IsNullOrEmpty(transacao.agencia) || transacao.valor <= 0) {
            return BadRequest(new { aprovada = false, mensagem = "Dados da transação são inválidos. Tente novamente!" });
        }

        // Fetch limite da conta desejada.
        var meuLimite = await _repository.Buscar(transacao.agencia, transacao.conta);

        if (meuLimite == null) {
            // Conta não cadastrada.
            return NotFound(new { aprovada = false, mensagem = "Cliente não possui limite cadastrado." });
        }

        // Conferir se o valor da transação está dentro do limite.
        if (meuLimite.limitePix >= transacao.valor) {
            // Aprovada! Descontar o valor do limite.
            meuLimite.limitePix -= transacao.valor;
            await _repository.Atualizar(meuLimite);
            return Ok(new {
                aprovada = true,
                mensagem = "Transação PIX aprovada.",
                limiteAtual = meuLimite.limitePix
            });
        } else {
            // Negada! Limite não é consumido.
            return Ok(new {
                aprovada = false,
                mensagem = "Transação negada por limite insuficiente.",
                limiteAtual = meuLimite.limitePix
            });
        }
    }
}
