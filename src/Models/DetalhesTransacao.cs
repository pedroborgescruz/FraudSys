
namespace FraudSys.Models {

    /// <summary>
    /// Encapsula os dados necessários para processar uma transação PIX.
    /// Essa classe combina os dados do limite da conta com o valor da transação.
    /// </summary>
    public class DetalhesTransacao {

        /// <summary>
        /// Define os detalhes do limite associados à conta da transação.
        /// Objeto do tipo Limite com atributos cpf, agencia, conta, limitePix.
        /// </summary>
        public Limite detalhesLimite {get; set;} = default!;

        /// <summary>
        /// Define o valor (R$) da transação PIX a ser processada.
        /// </summary>
        public decimal valorTransacao {get; set;} = default!;

    }
}
