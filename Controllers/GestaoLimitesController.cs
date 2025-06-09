using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FraudSys.Models;
using FraudSys.Repositories;

namespace FraudSys.Controllers;

/// <summary>
/// Controla as ações relacionadas à gestão de limites de contas (cadastro, 
/// busca, atualização e processamento de transações).
/// </summary>
public class GestaoLimitesController : Controller {
    private readonly ILogger<GestaoLimitesController> _logger;
    private readonly ILimiteRepository _repository;

    public GestaoLimitesController(ILogger<GestaoLimitesController> logger, ILimiteRepository repository) {
        _logger = logger;
        _repository = repository;
    }

    /// <summary>
    /// Exibe a view inicial com a lista de todos os limites cadastrados.
    /// </summary>
    /// <returns>A view Index com a lista de limites.</returns>
    public async Task<IActionResult> Index() {
        var todosOsLimites = await _repository.BuscarTodos();
        return View(todosOsLimites);
    }

    /// <summary>
    /// Exibe a view de cadastro de novo limite.
    /// </summary>
    /// <returns>A view CadastrarLimite.</returns>
    public IActionResult CadastrarLimite() {
        return View();
    }

    /// <summary>
    /// Exibe a view de busca de limite.
    /// </summary>
    /// <returns>A view BuscarLimite.</returns>
    public IActionResult BuscarLimite() {
        return View();
    }

    /// <summary>
    /// Exibe a view para atualizar o limite de uma conta específica.
    /// </summary>
    /// <param name="agencia">O número da agência da conta.</param>
    /// <param name="conta">O número da conta.</param>
    /// <returns>A view AtualizarLimite com os dados da conta ou NotFound se não for encontrada.</returns>
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

    /// <summary>
    /// Exibe a view para remover o limite de uma conta.
    /// </summary>
    /// <param name="agencia">O número da agência da conta.</param>
    /// <param name="conta">O número da conta.</param>
    /// <returns>A view RemoverLimite com os dados da conta.</returns>
    public async Task<IActionResult> RemoverLimite(string agencia, string conta) {
        if (string.IsNullOrEmpty(agencia) || string.IsNullOrEmpty(conta)) {
            return View();
        }

        var limite = await _repository.Buscar(agencia, conta);
        if (limite == null) {
            ViewBag.Message = "Não há limite cadastrado para a conta inserida";
            return View();
        }

        return View(limite);
    }

    /// <summary>
    /// Exibe os detalhes de uma conta e o endpoint para simular uma transação PIX.
    /// </summary>
    /// <param name="agencia">O número da agência da conta.</param>
    /// <param name="conta">O número da conta.</param>
    /// <returns>A view DetalhesLimite com os dados da conta ou NotFound se não for encontrado um cadastro.</returns>
    public async Task<IActionResult> DetalhesLimite(string agencia, string conta) {
        // Validar dados da transação.
        if (string.IsNullOrEmpty(agencia) || string.IsNullOrEmpty(conta)) {
            return View();
        }

        // Checar se o cadastro existe no banco de dados.
        var limite = await _repository.Buscar(agencia, conta);
        if (limite == null) {
            return NotFound();
        }
        
        // Instanciar um objeto da classe DetalhesTransacao
        var detalhes = new DetalhesTransacao {
            detalhesLimite = limite,
            valorTransacao = 0
        };

        return View(detalhes);
    }

    /// <summary>
    /// Exibe a view de erro padrão da aplicação.
    /// </summary>
    /// <returns>A view de erro com o RequestId.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Processa o envio do formulário de cadastro de um novo limite.
    /// </summary>
    /// <param name="limite">O objeto Limite preenchido com os dados do 
    /// formulário.</param>
    /// <returns>Redireciona para a view Index em caso de sucesso ou retorna a 
    /// view com uma mensagem de erro.</returns>
    [HttpPost]
    public async Task<IActionResult> Cadastrar(Limite limite)  {
        var limiteExistente = await _repository.Buscar(limite.agencia, limite.conta);

        if (limiteExistente != null) { // Limite já existe
            ViewBag.Message = "Já existe um limite cadastrado para esta conta.";
            return View("CadastrarLimite");
        }
        await _repository.Cadastrar(limite);
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Processa a busca por um limite de conta com base na primary key 
    /// fornecida.
    /// </summary>
    /// <param name="limite">Objeto contendo a agência e conta a serem 
    /// buscadas.</param>
    /// <returns>A view BuscarLimite com o resultado da busca ou uma mensagem 
    /// de erro.</returns>
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

    /// <summary>
    /// Processa a atualização do valor do limite de uma conta.
    /// </summary>
    /// <param name="limite">Objeto contendo a agência, conta e o 
    /// novo valor do limite.</param>
    /// <returns>A view AtualizarLimite com uma mensagem de 
    /// sucesso ou erro.</returns>
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

    /// <summary>
    /// Processa a remoção de um registro de limite de conta.
    /// </summary>
    /// <param name="agencia">O número da agência da conta a ser 
    /// removida.</param>
    /// <param name="conta">O número da conta a ser removida.</param>
    /// <returns>Redireciona para a view Index em caso de sucesso ou retorna a 
    /// view com uma mensagem de erro.</returns>
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

    /// <summary>
    /// Processa uma transação PIX, validando-a contra o limite da conta e 
    /// atualizando o saldo se aprovada.
    /// </summary>
    /// <param name="transacao">Os detalhes da transação, incluindo o cadastro
    /// de limite PIX e o valor.</param>
    /// <returns>A view DetalhesLimite com o resultado da transação
    /// (aprovada ou negada) e o limite atualizado.</returns>
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
            return View("DetalhesLimite", transacao);
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
