using System;
using MiniBanco.ConsoleApp.Models;
using MiniBanco.ConsoleApp.Models.Enums;

namespace MiniBanco.ConsoleApp.Services;

public class RelatorioService : IRelatorioService
{

    public void ExibirRelatorioContas(List<Conta> contas)
    {
        Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    RELATÓRIO DE CONTAS                  ║");
        Console.WriteLine("╚═════════════════════════════════════════════════════════╝");

        if (contas.Count == 0)
        {
            Console.WriteLine("Nenhuma conta aberta até o momento.");
            return;
        }

        var motorAnalise = new MotorAnalise();

        for (int x = 0; x < contas.Count; x++)
        {
            {
                Console.WriteLine($" [{x + 1} / {contas.Count}] PROCESSANDO CONTA");
                Console.WriteLine("-----------------------------------------------------------");
                DadosDaConta(contas[x]);
                MovimentacoesRegistradas(contas[x]);
                ResumoPontuacao(contas[x], motorAnalise);
                RelatorioPendenciaRecomendacaoGerencia(contas[x]);
            }
            Console.WriteLine("-----------------------------------------------------------");
        }
        Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                FIM DO RELATÓRIO DE CONTAS               ║");
        Console.WriteLine("╚═════════════════════════════════════════════════════════╝");
    }

    public void DadosDaConta(Conta conta)
    {
        String tipoDaConta = "";
        String atributoEspecifico = "";
        String empregador = "";

        if (conta is ContaCorrente contaCorrente)
        {
            tipoDaConta = "Conta Corrente";
            atributoEspecifico = $"Limite do Cheque Especial: R$ {contaCorrente.LimiteChequeEspecial:F2}";
        }
        else if (conta is ContaPoupanca contaPoupanca)
        {
            tipoDaConta = "Conta Poupança";
            atributoEspecifico = $"Rendimento: {contaPoupanca.TaxaRendimentoMensal}% ao mês | Aniversário: dia {contaPoupanca.DiaAniversario}";
        }
        else if (conta is ContaSalario contaSalario)
        {
            tipoDaConta = "Conta Salário";
            atributoEspecifico = $"Limite de Saques Mensais: {contaSalario.LimiteSaquesMensais}";
            empregador = contaSalario.Empregador;
        }

        Console.WriteLine("▶  DADOS DA CONTA:");
        Console.WriteLine($" - Tipo: {tipoDaConta}");
        Console.WriteLine($" - Titular: {conta.Titular} | Conta: {conta.Numero}");
        if (conta is ContaSalario)
        {
            Console.WriteLine($" - Empregador: {empregador}");
        }
        Console.WriteLine($" - Atributo Específico: {atributoEspecifico}");
        Console.WriteLine($" - Saldo Atual: R$ {conta.Saldo:F2}");
        Console.WriteLine();
    }

    public void MovimentacoesRegistradas(Conta conta)
    {
        Console.WriteLine($"▶  MOVIMENTAÇÕES REGISTRADAS ({conta.Extrato.Count} TRANSAÇÕES):");

        foreach (var c in conta.Extrato)
        {
            string status = "[✔]";
            int pontos = 10;

            if (c.Tipo == TipoTransacao.Saque)
            {
                status = "[!]";
                pontos = 5;
            }
            else if (c.Tipo == TipoTransacao.Tarifa)
            {
                status = "[✖]";
                pontos = 0;
            }
            Console.WriteLine($"{status} {c.Descricao,-30} {c.Tipo,-10}: R$ {c.Valor,8:F2} ({pontos} pts)");
        }
        Console.WriteLine();
    }

    public void ResumoPontuacao(Conta conta, IMotorAnalise motorAnalise)
    {
        var (calculoPontuacao, quantidadeTransacao) = motorAnalise.CalcularPontuacao(conta);
        int pontuacaoMaxima = quantidadeTransacao * 10;
        double indiceSaude = motorAnalise.CalcularIndiceSaude(conta);
        ClassificacaoConta classificacao = motorAnalise.ClassificarConta(conta);
        String textoClassificacao = classificacao == ClassificacaoConta.Saudavel ? "[ CONTA SAUDÁVEL ]"
                                     : classificacao == ClassificacaoConta.Risco ? "[ CONTA EM RISCO ]"
                                                                                 : "[ CONTA EM ATENÇÃO ]";

        Console.WriteLine($"▶  RESUMO DA PONTUAÇÃO: ");
        Console.WriteLine($" - Pontuação Atingida: {calculoPontuacao} de {pontuacaoMaxima} pontos possíveis");
        Console.WriteLine($" - Índice de Saúde: {indiceSaude:F1}%");
        Console.WriteLine($" - Classificação Final: {textoClassificacao}");
        Console.WriteLine();
    }

    public void RelatorioPendenciaRecomendacaoGerencia(Conta conta)
    {
        Console.WriteLine("▶  RELATÓRIO DE PENDÊNCIAS E APONTAMENTOS:");

        List<Transacao> tarifasEncontradas = new List<Transacao>();
        List<Transacao> saquesEncontrados = new List<Transacao>();

        foreach (var t in conta.Extrato)
        {
            if (t.Tipo == TipoTransacao.Tarifa)
            {
                tarifasEncontradas.Add(t);
            }
            else if (t.Tipo == TipoTransacao.Saque)
            {
                saquesEncontrados.Add(t);
            }
        }

        if (tarifasEncontradas.Count > 0)
        {
            Console.WriteLine("   🔴 ITENS CRÍTICOS (TARIFAS):");
            foreach (var t in tarifasEncontradas)
            {
                Console.WriteLine($"    - {t.Descricao}: R$ {t.Valor:F2} (Avaliar isenção ou migração para pacote de serviços)");
            }
        }

        if (saquesEncontrados.Count > 0)
        {
            Console.WriteLine("   🟡 ITENS DE ATENÇÃO (SAQUES):");
            foreach (var t in saquesEncontrados)
            {
                Console.WriteLine($"    - {t.Descricao}: R$ {t.Valor:F2} (Retirada que exige acompanhamento)");
            }
        }

        if (tarifasEncontradas.Count == 0 && saquesEncontrados.Count == 0)
        {
            Console.WriteLine("🟢 Nenhuma pendência identificada. Oferecer produtos de investimento ao cliente.");
        }
    }

}
