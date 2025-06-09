using FraudSys.Models;

namespace FraudSys.Repositories {
    /// <summary>
    /// Define os métodos para operações de acesso ao banco de dados 
    /// para a tabela "limites" no DynamoDB.
    /// </summary>
    public interface ILimiteRepository {
        /// <summary>
        /// Cadastra um novo limite no banco de dados.
        /// </summary>
        /// <param name="limite">O objeto Limite a ser cadastrado.</param>
        Task Cadastrar(Limite limite);

        /// <summary>
        /// Atualiza um limite existente no banco de dados.
        /// </summary>
        /// <param name="limite">O objeto Limite a ser atualizado.</param>
        Task Atualizar (Limite limite);

        /// <summary>
        /// Busca um limite específico com base na primary key (agência e conta).
        /// </summary>
        /// <param name="agencia">O número da agência da conta.</param>
        /// <param name="conta">O número da conta.</param>
        /// <returns>O objeto Limite encontrado ou null se não existir.</returns>
        Task<Limite?> Buscar(string agencia, string conta);

        /// <summary>
        /// Busca todos os limites cadastrados no banco de dados.
        /// </summary>
        /// <returns>Uma coleção de todos os limites na DynamoDB.</returns>
        Task<IEnumerable<Limite>> BuscarTodos();

        /// <summary>
        /// Remove um limite do banco de dados com base na primary key (agência 
        /// e conta).
        /// </summary>
        /// <param name="agencia">O número da agência da conta a ser removida.</param>
        /// <param name="conta">O número da conta a ser removida.</param>
        Task Remover(string agencia, string conta); 
    }
}