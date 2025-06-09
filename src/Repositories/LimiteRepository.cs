using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DataModel;
using FraudSys.Models;

namespace FraudSys.Repositories {
    /// <summary>
    /// Implementa o repositório para gerenciamento de limites, utilizando 
    /// AWS DynamoDB como a base de dados.
    /// </summary>
    public class LimiteRepository : ILimiteRepository {
        
        private readonly IDynamoDBContext _context;

        /// <summary>
        /// Construtor; inicializa uma instância da classe "LimiteRepository".
        /// </summary>
        /// <param name="context">O contexto do DynamoDB injetado para acesso
        /// ao banco de dados.
        /// </param>
        public LimiteRepository(IDynamoDBContext context) {
            _context = context;
        }

        public async Task Cadastrar(Limite limite) {
            // SaveAsync (DynamoDBContext Doc): criar um novo item.
            var validationContext = new ValidationContext(limite, null, null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(limite, validationContext, validationResults, true);

            if (!isValid) {
                throw new ValidationException($"O objeto Limite é inválido: {string.Join(", ", validationResults.Select(e => e.ErrorMessage))}");
            }
            await _context.SaveAsync(limite);
        }

        public async Task Atualizar(Limite limite) {
            // SaveAsync (DynamoDBContext Doc): substituir um item.
            await _context.SaveAsync(limite);
        }

        public async Task<Limite?> Buscar(string agencia, string conta) {
            // QueryAsync: buscar uma linha pela partition key (agencia)
            // e sort key (conta).
            var list = await _context.QueryAsync<Limite>(agencia, Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, new object[] { conta }).GetRemainingAsync();

            return list.FirstOrDefault();
        }

        public async Task<IEnumerable<Limite>> BuscarTodos() {
            // ScanAsync (DynamoDBContext Doc): ler todos os itens da tabela.
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Limite>(conditions).GetRemainingAsync();
        }

        public async Task Remover(string agencia, string conta) {
            // DeleteAsync (DynamoDBContext Doc): remover uma entity específica
            // pela sua primary key.
            await _context.DeleteAsync<Limite>(agencia, conta);
        }
    }
}