using Amazon.DynamoDBv2.DataModel;

namespace FraudSys.Models {

    [DynamoDBTable("limites")]
    public class Limite {

        [DynamoDBProperty]
        public string cpf {get; set;} = default!; 
        [DynamoDBHashKey("agencia")]
        public string agencia {get; set;} = default!;
        [DynamoDBRangeKey("conta")]
        public string conta {get; set;} = default!;
        [DynamoDBProperty]
        public decimal limitePix {get; set;} = default!;

    }
}
