using System;
using MiniBanco.ConsoleApp.Models;
using MiniBanco.ConsoleApp.Models.Enums;

namespace MiniBanco.ConsoleApp.Services;

public class MotorAnalise : IMotorAnalise
{
    public int CalculoPontuacao { get; private set; }

    public (int calculoPontuacao, int quantidadeTransacao) CalcularPontuacao(Conta conta)
    {
        this.CalculoPontuacao = 0;
        int quantidadeTransacao = 0;

        foreach (var t in conta.Extrato)
        {
            this.CalculoPontuacao += t.Tipo == TipoTransacao.Deposito ? 10
                              : t.Tipo == TipoTransacao.Saque ? 5
                              : 0;

            quantidadeTransacao++;
        }

        return (this.CalculoPontuacao, quantidadeTransacao);
    }

    public double CalcularIndiceSaude(Conta conta)
    {
        CalcularPontuacao(conta);

        int totalTransacoes = conta.Extrato.Count;
        int pontuacaoMaxima = totalTransacoes * 10;

        if (pontuacaoMaxima == 0)
        {
            return 0.0;
        }

        double indiceSaude = ((double)CalculoPontuacao / pontuacaoMaxima) * 100.0;

        return indiceSaude;
    }

    public ClassificacaoConta ClassificarConta(Conta conta)
    {
        double percentual = CalcularIndiceSaude(conta);

        if (percentual >= 90.0)
        {
            return ClassificacaoConta.Saudavel;
        }
        else if (percentual >= 60.0)
        {
            return ClassificacaoConta.Atencao;
        }
        else
        {
            return ClassificacaoConta.Risco;
        }
    }
}