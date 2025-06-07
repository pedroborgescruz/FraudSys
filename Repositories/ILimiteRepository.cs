using FraudSys.Models;

namespace FraudSys.Repositories {
    public interface ILimiteRepository
    {
        Task Cadastrar(Limite limite);
        Task Atualizar (Limite limite);
        Task<Limite?> Buscar(string agencia, string conta);
        Task<IEnumerable<Limite>> BuscarTodos();
        Task Remover(string agencia, string conta); 
    }
}