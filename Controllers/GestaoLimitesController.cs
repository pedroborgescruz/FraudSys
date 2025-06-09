using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FraudSys.Models;
using FraudSys.Repositories;

namespace FraudSys.Controllers;

public class GestaoLimitesController : Controller
{
    private readonly ILogger<GestaoLimitesController> _logger;
    private readonly ILimiteRepository _repository;

    public GestaoLimitesController(ILogger<GestaoLimitesController> logger, ILimiteRepository repository) {
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

    public async Task<IActionResult> AtualizarLimite(string agencia, string conta) {
        if (string.IsNullOrEmpty(agencia) || string.IsNullOrEmpty(conta)) {
            return View();
        }
        var limite = await _repository.Buscar(agencia, conta);
        if (limite == null) {
            return NotFound();
        }

        return View(limite);
    }

    public async Task<IActionResult> RemoverLimite(string agencia, string conta) {
        if (string.IsNullOrEmpty(agencia) || string.IsNullOrEmpty(conta)) {
            return View();
        }
        var limite = await _repository.Buscar(agencia, conta);
        if (limite == null) {
            return NotFound();
        }

        return View(limite);
    }

    public async Task<IActionResult> DetalhesLimite(string agencia, string conta) {
        if (string.IsNullOrEmpty(agencia) || string.IsNullOrEmpty(conta)) {
            return View();
        }
        var limite = await _repository.Buscar(agencia, conta);
        if (limite == null) {
            return NotFound();
        }
        
        var detalhes = new DetalhesTransacao {
            detalhesLimite = limite,
            valorTransacao = 0
        };

        return View(detalhes);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(Limite limite)  {
        await _repository.Cadastrar(limite);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Buscar(Limite limite) {
        var res = await _repository.Buscar(limite.agencia, limite.conta);

        if (res != null) {
            return View("BuscarLimite", res);
        } else {
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
        } else {
            meuLimite.limitePix = limite.limitePix;
            await _repository.Atualizar(meuLimite);

            ViewBag.Message = "Sucesso! Limite atualizado.";
        }

        return View("AtualizarLimite");
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
    public async Task<IActionResult> ProcessarPix(DetalhesTransacao transacao) {
        // Validar dados da transação.
        if (transacao == null || string.IsNullOrEmpty(transacao.detalhesLimite.conta) || string.IsNullOrEmpty(transacao.detalhesLimite.agencia) || transacao.valorTransacao <= 0) {
            ViewBag.TransacaoFeita = false;
            ViewBag.Message = "Dados da transação são inválidos. Tente novamente!";
            return View("DetalhesLimite", transacao);
        }

        // Fetch limite da conta desejada.
        var meuLimite = await _repository.Buscar(transacao.detalhesLimite.agencia, transacao.detalhesLimite.conta);

        if (meuLimite == null) {
            // Conta não cadastrada.
            ViewBag.TransacaoFeita = false;
            ViewBag.Message = "Cliente não possui limite cadastrado.";
        }

        transacao.detalhesLimite = meuLimite;

        // Conferir se o valor da transação está dentro do limite.
        if (meuLimite.limitePix >= transacao.valorTransacao) { // Transação aprovada.
            meuLimite.limitePix -= transacao.valorTransacao;
            await _repository.Atualizar(meuLimite);
            transacao.detalhesLimite = meuLimite;
            ViewBag.Message = "Sucesso! Transação realizada.";
            ViewBag.TransacaoFeita = true;
        } else { // Transação não autorizada.
            ViewBag.TransacaoFeita = false;
            ViewBag.Message = "Transação não realizada. Limite não suficiente.";
        }
        return View("DetalhesLimite", transacao);
    }
}
