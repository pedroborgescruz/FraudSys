using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations; 

namespace FraudSys.Models {

    /// <summary>
    /// Modela os dados de limite de transação PIX para uma conta de cliente.
    /// </summary>
    [DynamoDBTable("limites")]
    public class Limite {

        /// <summary>
        /// Define o CPF do titular da conta.
        /// </summary>
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter 11 dígitos.")]
        [DynamoDBProperty]
        public string cpf {get; set;} = default!; 

        /// <summary>
        /// Define o número da agência da conta.
        /// Essa é a Partition Key da tabela no DynamoDB.
        /// </summary>
        [Required(ErrorMessage = "A agência é obrigatória.")]
        [DynamoDBHashKey("agencia")]
        public string agencia {get; set;} = default!;

        /// <summary>
        /// Define o número da conta do titular.
        /// Essa é a Sort Key da tabela no DynamoDB.
        /// </summary>
        [Required(ErrorMessage = "A conta é obrigatória.")]
        [DynamoDBRangeKey("conta")]
        public string conta {get; set;} = default!;

        /// <summary>
        /// Define o valor do limite disponível para transações PIX.
        /// </summary>
        [Required(ErrorMessage = "O limite para PIX é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O limite deve ser um valor positivo.")]
        [DynamoDBProperty]
        public decimal limitePix {get; set;} = default!;

    }
}
