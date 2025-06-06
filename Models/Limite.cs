using System.ComponentModel.DataAnnotations;

namespace FraudSys.Models;

/// <summary>
/// Classe <c>Limite</c> modela o limite PIX associado à conta de um cliente.
/// </summary>
public class Limite {
    
    public required string cpf {get; set;}     
    public required string agencia {get; set;}
    public required string conta {get; set;}
    public required decimal limitePix {get; set;}

    public Limite() { }

    public Limite(string cpf, string agencia, string conta, int limitePix) {
        this.cpf = cpf;
        this.agencia = agencia;
        this.conta = conta;
        this.limitePix = limitePix;
    }

    public static Limite Buscar(string agencia, string conta) {
        throw new NotImplementedException();
    }
    
    public static void Atualizar(string agencia, string conta, int limitePix) {
        throw new NotImplementedException();
    }

    public static void Remover(string numeroAgencia, string conta) {
        new NotImplementedException();
    }
    
}