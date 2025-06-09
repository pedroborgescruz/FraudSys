using FraudSys.Models;
using FraudSys.Repositories;

namespace FraudSys.Models
{
    public class LimiteService {
        private readonly ILimiteRepository _limiteRepository;

        public LimiteService(ILimiteRepository limiteRepository) {
            _limiteRepository = limiteRepository;
        }

        public async Task<bool> PodeRealizarPix(DetalhesTransacao transacao) {
            var limite = await _limiteRepository.Buscar(transacao.detalhesLimite.agencia, transacao.detalhesLimite.conta);
            if (limite == null) {
                throw new Exception("Conta não encontrada");
            }

            if (transacao.valorTransacao >= transacao.detalhesLimite.limitePix) {
                // Atualiza o limite
                limite.limitePix -= transacao.valorTransacao;
                await _limiteRepository.Atualizar(limite);
                return true;
            }

            return false;
        }
    }
}
