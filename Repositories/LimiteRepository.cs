using Amazon.DynamoDBv2.DataModel;
using FraudSys.Models;

namespace FraudSys.Repositories {
    public class LimiteRepository : ILimiteRepository {
        
        private readonly IDynamoDBContext _context;

        public LimiteRepository(IDynamoDBContext context) {
            _context = context;
        }

        public async Task Cadastrar(Limite limite) {
            await _context.SaveAsync(limite);
        }

        public async Task Atualizar(Limite limite) {
            await _context.SaveAsync(limite);
        }

        public async Task<Limite?> Buscar(string agencia, string conta) {
            var list = await _context.QueryAsync<Limite>(agencia, Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, new object[] { conta }).GetRemainingAsync();

            return list.FirstOrDefault();
        }

        public async Task Remover(string agencia, string conta) {
            await _context.DeleteAsync<Limite>(agencia, conta);
        }
    }
}