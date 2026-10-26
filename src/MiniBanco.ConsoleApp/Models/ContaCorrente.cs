using System;
using MiniBanco.ConsoleApp.Exceptions;
using MiniBanco.ConsoleApp.Models.Enums;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaCorrente : Conta
    {
        public decimal LimiteChequeEspecial { get; private set; }

        public ContaCorrente(string titular, int numero, decimal saldoInicial, decimal limiteChequeEspecial)
            : base(titular, numero, saldoInicial)
        {
            if (limiteChequeEspecial < 0)
            {
                throw new ValorInvalidoException("O limite do cheque especial não pode ser negativo.");
            }

            this.LimiteChequeEspecial = limiteChequeEspecial;
        }

        public override void Sacar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ValorInvalidoException("O valor da transação deve ser maior que zero.");
            }

            if (valor > this.Saldo + this.LimiteChequeEspecial)
            {
                throw new SaldoInsuficienteException("Saldo e limite de cheque especial insuficientes para realizar o saque.");
            }

            this.Saldo -= valor;
            RegistrarTransacao("Saque", TipoTransacao.Saque, valor);
        }

        public override void AplicarFechamentoMensal()
        {
            const decimal tarifaManutencao = 10.00m;

            this.Saldo -= tarifaManutencao;
            RegistrarTransacao("Tarifa de manutenção", TipoTransacao.Tarifa, tarifaManutencao);
        }
    }
}