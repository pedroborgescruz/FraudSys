# Sistema de Gestão de Limites (FraudSys) - Desafio BTG Pactual Tech

Este repositório contém o código para uma aplicação ASP.NET Core MVC desenvolvida como parte do processo seletivo para Desenvolvedor Full Stack no BTG Pactual. A aplicação gerencia limites de transações PIX para contas de clientes.

## Funcionalidades

- Cadastro, consulta, atualização e remoção de limites PIX de contas.
- Processamento de transações PIX com validação de limite.
- Listagem de todas as contas com limites cadastrados.

## Tecnologias Utilizadas

- **Backend**: .NET 8, ASP.NET Core MVC
- **Banco de Dados**: AWS DynamoDB

## ⚙Configuração do Ambiente

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/pedroborgescruz/FraudSys.git
    cd FraudSys
    ```

2.  **Configure as credenciais:**
    - As credenciais de acesso temporárias da AWS (`Access Key ID` e `Secret Access Key`) foram enviadas por e-mail.
    - Na raiz do projeto, renomeie o arquivo `.env.example` para `.env`.
    - Abra o arquivo `.env` e cole as credenciais fornecidas.

    O arquivo `.env` deve ficar assim:
    ```bash
    AWS_ACCESS_KEY_ID="AKI...EXEMPLO"
    AWS_SECRET_ACCESS_KEY="A+b/C...exemplo"
    AWS_REGION="us-east-2"
    ```

## Como Executar o Projeto

1.  **Pré-requisitos**:
    * .NET 8 SDK
    * Credenciais da AWS configuradas no seu ambiente (a aplicação usa o SDK para se autenticar).

2.  **Configuração**:
    * Navegue até a pasta `FraudSys/src` do projeto no seu terminal.

3.  **Execução**:
    * Execute o comando `dotnet run` no seu terminal.
    * Acesse a aplicação em `https://localhost:[PORT]`.

## Testes Unitários

Atualmente, este projeto possui testes unitários implementados para o controlador `GestaoLimitesController` em `tests`, cobrindo os seguintes cenários:

* CadastrarLimiteSucesso: Verifica se o limite é cadastrado corretamente.
* CadastrarLimiteInvalido: Simula um erro de validação (CPF inválido) e verifica se o método retorna corretamente uma View de erro.

Se eu estivesse trabalhando de forma mais integral no projeto (como se fosse no BTG), a seguir estão alguns outros testes que eu planejaria adicionar para verificar a robustez da plataforma antes de enviar para aprovação:

1. Edição de limite: Verificar se o método de edição chama o repositório corretamente e trata cenários válidos e inválidos (por exemplo, CPF inválido ou conta inexistente).
2. Exclusão de limite: Garantir que a exclusão de um limite chama o método de repositório correto e lida com casos de erro (como tentar excluir um limite inexistente).
3. Detalhes: Verificar se o método retorna os dados corretos para exibir detalhes de um limite, e também como lida com entradas inválidas ou limites inexistentes.
4. Buscar limite: Verificar se o método retorna os dados corretos para um cenário válido (limite recém cadastrado na base de dados) e um cenário inválido (limite não existe na base de dados).
5. Checar estado: Realizar uma série de operações nos dados de um limite cadastrado (por exemplo, Cadastrar > Buscar > Atualizar > Atualizar > Realizar PIX; etc.) e verificar se os dados permanecem sólidos a todo ponto.
6. Checar limite deletado é realmente deletado: Deletar um limite no banco de dados e verificar que ele não consta mais na base.
