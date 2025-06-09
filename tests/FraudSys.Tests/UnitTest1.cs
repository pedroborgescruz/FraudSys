using Xunit;
using Moq;
using FraudSys.Controllers;
using FraudSys.Repositories;
using FraudSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FraudSys.Tests
{
    public class GestaoLimitesControllerTests {
        // Declare as variáveis no escopo da classe
        private readonly Mock<ILimiteRepository> _mockRepo;
        private readonly GestaoLimitesController _controller;

        // Use o construtor da classe de teste para preparar o ambiente
        public GestaoLimitesControllerTests() {
            _mockRepo = new Mock<ILimiteRepository>();
            var mockLogger = new Mock<ILogger<GestaoLimitesController>>();
            _controller = new GestaoLimitesController(mockLogger.Object, _mockRepo.Object);
        }

        /// <summary>
        /// Teste para verificar se um usuário foi corretamente cadastrado.
        /// </summary>
        [Fact]
        public void Test1_CadastrarLimite() {
            var novoLimite = new Limite { 
                cpf = "123.456.789-12",
                agencia = "0123",
                conta = "45678",
                limitePix = 1000 };

            var result = _controller.Cadastrar(novoLimite);

            // Verificar se o método `Criar` do repositório foi chamado exatamente uma vez.
            _mockRepo.Verify(repo => repo.Cadastrar(It.IsAny<Limite>()), Times.Once);
        }
    }
}