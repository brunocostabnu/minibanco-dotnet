🚀 MiniBanco .NET

Um sistema de gestão bancária em consola desenvolvido em C# / .NET, focado na aplicação prática de Programação Orientada a Objetos (POO), padrões de código limpo (SOLID) e tratamento robusto de dados.

📋 Sobre o Projeto

O MiniBanco permite gerir diferentes tipos de contas bancárias, registar movimentações (depósitos, saques e tarifas), aplicar fechamentos mensais com regras de negócio específicas e gerar um relatório gerencial com análise de saúde financeira da conta.

🌟 Principais Funcionalidades

Abertura de Contas: Suporte para Conta Corrente, Conta Poupança e Conta Salário.

Gestão de Movimentações: Validação e registo de depósitos e saques com historico completo de extrato.

Fechamento Mensal:
    Conta Corrente: Aplicação automatizada de tarifa de manutenção de R$ 10,00.   
    Conta Poupança: Cálculo e creditação de rendimento mensal.   
    Conta Salário: Reinício do contador mensal de saques.   

Motor de Análise e Saúde da Conta:
    Cálculo de pontuação com base no histórico de movimentações.   
    Classificação da conta em Saudável, Em Atenção ou Em Risco.   
    Apontamento de itens críticos e recomendações de gestão.   

🏗️ Arquitetura e Estrutura do Código

O projeto foi estruturado seguindo os princípios de separação de responsabilidades (SoC):

src/MiniBanco.ConsoleApp/
├── Exceptions/                  # Exceções customizadas do domínio
│   ├── DataAniversarioException.cs
│   ├── EntradaInvalidaException.cs
│   ├── SaldoInsuficienteException.cs
│   └── ValorInvalidoException.cs
├── Models/                      # Modelos de domínio e hierarquia de contas
│   ├── Enums/
│   │   ├── ClassificacaoConta.cs
│   │   └── TipoTransacao.cs
│   ├── Conta.cs                 # Classe base abstrata
│   ├── ContaCorrente.cs         # Herança e regras de cheque especial
│   ├── ContaPoupanca.cs         # Herança e regras de rendimento
│   ├── ContaSalario.cs          # Herança e tarifação de saques excedentes
│   └── Transacao.cs             # Modelo de registo de extrato
├── Services/                    # Regras de negócio e relatórios
│   ├── Interfaces/
│   │   ├── IMotorAnalise.cs     # Contrato para análise de pontuação/saúde
│   │   └── IRelatorioService.cs # Contrato para exibição e relatórios
│   ├── MotorAnalise.cs          # Implementação do motor de análise
│   └── RelatorioService.cs      # Lógica de formatação do relatório gerencial
└── Program.cs                   # Menu interativo do sistema

🛠️ Tecnologias e Conceitos Utilizados

Linguagem: C# (.NET 8+)

POO: Abstração, Herança, Polimorfismo (virtual / override) e Encapsulamento (private set / protected set).   

Boas Práticas:
    Inversão de Dependência (DIP) com o uso de Interfaces (IMotorAnalise, IRelatorioService).   
    Tratamento de exceções específicas para validação de fluxos de entrada e regras de saldo.   
    Uso do operador de coalescência nula (??) e tratamento de Nullable Reference Types.

🚀 Como Executar o Projeto

Pré-requisitos
SDK do .NET Core / .NET 8+ instalado.
Visual Studio Code (ou Visual Studio).

Passos

Clonar o repositório:
git clone https://github.com/brunocostabnu/minibanco-dotnet.git
cd minibanco-dotnet

Navegar até a pasta da aplicação de console:
cd src/MiniBanco.ConsoleApp

Executar o projeto:
dotnet run