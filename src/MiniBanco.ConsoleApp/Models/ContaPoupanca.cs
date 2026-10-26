using System;
using MiniBanco.ConsoleApp.Exceptions;
using MiniBanco.ConsoleApp.Models.Enums;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaPoupanca : Conta
    {
        public double TaxaRendimentoMensal { get; private set; }
        public int DiaAniversario { get; private set; }
        public ContaPoupanca(string titular, int numero, decimal saldoInicial, double taxaRendimentoMensal, int diaAniversario)
            : base(titular, numero, saldoInicial)
        {
            if (taxaRendimentoMensal < 0)
            {
                throw new ValorInvalidoException("A taxa de rendimento não pode ser negativa.");
            }

            if (diaAniversario < 1 || diaAniversario > 31)
            {
                throw new DataAniversarioException("O dia de aniversário deve ser entre 1 e 31.");
            }

            this.TaxaRendimentoMensal = taxaRendimentoMensal;
            this.DiaAniversario = diaAniversario;
        }

        public override void AplicarFechamentoMensal()
        {
            decimal depositoDeRendimento = this.Saldo * (decimal)(this.TaxaRendimentoMensal / 100.0);

            if (depositoDeRendimento > 0)
            {
                this.Saldo += depositoDeRendimento;
                RegistrarTransacao("Rendimento mensal", TipoTransacao.Deposito, depositoDeRendimento);
            }
        }
    }
}