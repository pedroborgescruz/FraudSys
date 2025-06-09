# Sistema de Gestão de Limites (FraudSys) - Desafio BTG Pactual Tech

Este repositório contém o código para uma aplicação ASP.NET Core MVC desenvolvida como parte do processo seletivo para Desenvolvedor Full Stack no BTG Pactual. A aplicação gerencia limites de transações PIX para contas de clientes.

## Funcionalidades

- Cadastro, consulta, atualização e remoção de limites PIX de contas.
- Processamento de transações PIX com validação de limite.
- Listagem de todas as contas com limites cadastrados.

## Tecnologias Utilizadas

- **Backend**: .NET 8, ASP.NET Core MVC
- **Banco de Dados**: AWS DynamoDB

## Como Executar o Projeto

1.  **Pré-requisitos**:
    * .NET 8 SDK
    * Credenciais da AWS configuradas no seu ambiente (a aplicação usa o SDK para se autenticar).

2.  **Configuração**:
    * Clone o repositório: `git clone https://github.com/pedroborgescruz/FraudSys.git`
    * Navegue até a pasta do projeto no seu terminal.

3.  **Execução**:
    * Execute o comando `dotnet run` no seu terminal.
    * Acesse a aplicação em `https://localhost:[PORT]`.
