using System;
using System.Collections.Generic;
using MiniBanco.ConsoleApp.Models;
using MiniBanco.ConsoleApp.Services;
using MiniBanco.ConsoleApp.Models.Enums;
using MiniBanco.ConsoleApp.Exceptions;
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<Conta> contas = new List<Conta>();
        bool executando = true;

        while (executando)
        {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         MINIBANCO .NET - MOTOR DE GESTÃO DE CONTAS      ║");
            Console.WriteLine("╚═════════════════════════════════════════════════════════╝");
            Console.WriteLine("1 - Abrir Nova Conta");
            Console.WriteLine("2 - Registrar Movimentação (Depósito/Saque)");
            Console.WriteLine("3 - Aplicar Fechamento Mensal");
            Console.WriteLine("4 - Exibir Relatório das Contas");
            Console.WriteLine("5 - Carregar Cenários de Teste (2 por tipo)");
            Console.WriteLine("0 - Sair");
            Console.WriteLine("══════════════════════════════════════════════════════════");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine() ?? string.Empty;
            Console.WriteLine();

            switch (opcao)
            {
                case "1":
                    AbrirNovaConta(contas);
                    break;

                case "2":
                    RegistrarMovimentacao(contas);
                    break;

                case "3":
                    AplicarFechamentoMensalContas(contas);
                    break;

                case "4":
                    IRelatorioService relatorioService = new RelatorioService();
                    relatorioService.ExibirRelatorioContas(contas);
                    break;

                case "5":
                    CarregarCenariosDeTeste(contas);
                    break;

                case "0":
                    executando = false;
                    Console.WriteLine("Encerrando o sistema. Até logo!");
                    break;

                default:
                    Console.WriteLine("Opção inválida! Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                    break;
            }

            if (opcao != "0")
            {
                Console.WriteLine("\nPressione qualquer tecla para retornar ao menu...");
                Console.ReadKey();
            }
        }
    }

    private static void AbrirNovaConta(List<Conta> contas)
    {
        try
        {
            Console.WriteLine("--- ABRIR NOVA CONTA ---");
            Console.WriteLine("Selecione o tipo de conta:");
            Console.WriteLine("1 - Conta Corrente");
            Console.WriteLine("2 - Conta Poupança");
            Console.WriteLine("3 - Conta Salário");
            Console.Write("Opção: ");
            string tipoConta = Console.ReadLine() ?? string.Empty;

            if (tipoConta != "1" && tipoConta != "2" && tipoConta != "3")
            {
                throw new EntradaInvalidaException("Tipo de conta inválido. Operação cancelada.");
            }

            Console.Write("Digite o nome do Titular: ");
            string titular = Console.ReadLine() ?? string.Empty;

            Console.Write("Digite o Número da Conta: ");
            int numero = int.Parse(Console.ReadLine() ?? string.Empty);

            Console.Write("Digite o Saldo Inicial: R$ ");
            decimal saldoInicial = decimal.Parse(Console.ReadLine() ?? string.Empty);

            Conta? novaConta = null;

            switch (tipoConta)
            {
                case "1":
                    Console.Write("Digite o valor do Limite: R$ ");
                    decimal limite = decimal.Parse(Console.ReadLine() ?? string.Empty);
                    novaConta = new ContaCorrente(titular, numero, saldoInicial, limite);
                    break;

                case "2":
                    Console.Write("Digite a Taxa de Rendimento (%): ");
                    double taxaRendimento = double.Parse(Console.ReadLine() ?? string.Empty);
                    Console.Write("Digite o Dia de Aniversário do cliente: ");
                    int diaAniversario = int.Parse(Console.ReadLine() ?? string.Empty);
                    novaConta = new ContaPoupanca(titular, numero, saldoInicial, taxaRendimento, diaAniversario);
                    break;

                case "3":
                    Console.Write("Digite o nome do Empregador: ");
                    string empregador = Console.ReadLine() ?? string.Empty;
                    Console.Write("Digite o limite de saques mensais: ");
                    int limiteSaquesMensais = int.Parse(Console.ReadLine() ?? string.Empty);
                    novaConta = new ContaSalario(titular, numero, saldoInicial, empregador, limiteSaquesMensais);
                    break;

            }
            if (novaConta != null)
            {
                contas.Add(novaConta);
                Console.WriteLine("\nConta cadastrada com sucesso!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }

    private static void RegistrarMovimentacao(List<Conta> contas)
    {
        try
        {
            Console.WriteLine("--- REGISTRAR MOVIMENTAÇÃO ---");

            if (contas.Count == 0)
            {
                Console.WriteLine("Nenhuma conta cadastrada para registrar movimentação.");
                return;
            }

            Console.Write("Digite o número da conta: ");
            int numero = int.Parse(Console.ReadLine() ?? string.Empty);

            Conta? contaEncontrada = null;

            foreach (var c in contas)
            {
                if (numero == c.Numero)
                {
                    contaEncontrada = c;
                    break;
                }
            }

            if (contaEncontrada == null)
            {
                Console.WriteLine("Conta não encontrada na lista.");
                return;
            }

            Console.WriteLine($"Titular: {contaEncontrada.Titular}");
            Console.WriteLine($"Saldo Atual: R$ {contaEncontrada.Saldo:F2}");

            if (contaEncontrada is ContaCorrente contaCorrente)
            {
                Console.WriteLine($"Limite Cheque Especial: {contaCorrente.LimiteChequeEspecial}");
            }
            
            Console.WriteLine("Escolha a operação:");
            Console.WriteLine("1 - Depósito");
            Console.WriteLine("2 - Saque");
            Console.Write("Opção: ");
            string operacao = Console.ReadLine() ?? string.Empty;

            if (operacao == "1")
            {
                decimal valor = SolicitarValorTransacao();

                Console.Write("Insira a descrição do depósito: ");
                string descricao = Console.ReadLine() ?? string.Empty;

                contaEncontrada.Depositar(valor, descricao);
                Console.WriteLine("Depósito realizado com sucesso!");
            }
            else if (operacao == "2")
            {
                decimal valor = SolicitarValorTransacao();

                contaEncontrada.Sacar(valor);
                Console.WriteLine("Saque realizado com sucesso!");
            }
            else
            {
                throw new EntradaInvalidaException("Opção inválida.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }

    private static decimal SolicitarValorTransacao()
    {
        Console.Write("Digite o valor: R$ ");
        decimal valor = decimal.Parse(Console.ReadLine() ?? string.Empty);

        return valor;
    }

    private static void AplicarFechamentoMensalContas(List<Conta> contas)
    {
        Console.WriteLine("--- APLICAR FECHAMENTO MENSAL ---");

        if (contas.Count == 0)
        {
            Console.WriteLine("Nenhuma conta cadastrada para aplicar o fechamento.");
            return;
        }

        foreach (var conta in contas)
        {
            conta.AplicarFechamentoMensal();
        }

        Console.WriteLine("Fechamento mensal aplicado com sucesso em todas as contas!");
    }

    private static void CarregarCenariosDeTeste(List<Conta> contas)
    {
        contas.Clear();

        Console.WriteLine("--- CARREGANDO CENÁRIOS DE TESTE ---");

        ContaCorrente c1 = new ContaCorrente("Ana Souza", 101, 1500.00m, 500.00m);
        c1.Depositar(500.00m, "Depósito Inicial em Dinheiro");
        c1.Sacar(200.00m); // Gera transação de saque
        c1.Extrato.Add(new Transacao("Tarifa de Manutenção de Conta", TipoTransacao.Tarifa, 15.00m));
        contas.Add(c1);

        ContaPoupanca c2 = new ContaPoupanca("Carlos Silva", 102, 3000.00m, 0.5, 10);
        c2.Depositar(1000.00m, "Aporte Mensal Poupança");
        c2.Depositar(500.00m, "Reembolso");
        contas.Add(c2);

        ContaSalario c3 = new ContaSalario("Mariana Costa", 103, 2500.00m, "Tech Solutions Ltda", 2);
        c3.Depositar(2500.00m, "Crédito de Salário");
        c3.Sacar(300.00m); // Saque dentro do limite permitido
        contas.Add(c3);

        ContaCorrente c4 = new ContaCorrente("Roberto Dias", 104, 5000.00m, 1000.00m);
        c4.Depositar(1500.00m, "Prestação de Serviços");
        c4.Depositar(1000.00m, "Bônus de Projetos");
        contas.Add(c4);

        Console.WriteLine("Cenários de teste carregados com sucesso! Pressione '4' no menu para exibir o relatório completo.");
    }
}