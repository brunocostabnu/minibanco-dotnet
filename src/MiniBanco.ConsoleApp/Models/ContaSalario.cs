using System;
using MiniBanco.ConsoleApp.Exceptions;
using MiniBanco.ConsoleApp.Models.Enums;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaSalario : Conta
    {
        public string Empregador { get; private set; }
        public int LimiteSaquesMensais { get; private set; }
        private int ContadorDeSaques;
        public ContaSalario(string titular, int numero, decimal saldoInicial, string empregador, int limiteSaquesMensais)
            : base(titular, numero, saldoInicial)
        {
            this.Empregador = empregador;
            this.LimiteSaquesMensais = limiteSaquesMensais;
        }

        public override void Sacar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ValorInvalidoException("O valor da transação deve ser maior que zero.");
            }

            const decimal valorTarifa = 5.00m;

            bool cobraTarifa = this.ContadorDeSaques >= this.LimiteSaquesMensais;

            decimal valorNecessario = cobraTarifa ? valor + valorTarifa : valor;

            if (valorNecessario > this.Saldo)
            {
                throw new SaldoInsuficienteException("Saldo insuficiente para realizar o saque.");
            }

            this.Saldo -= valor;
            RegistrarTransacao("Saque", TipoTransacao.Saque, valor);

            if (cobraTarifa)
            {
                this.Saldo -= valorTarifa;
                RegistrarTransacao("Tarifa de R$ 5,00", TipoTransacao.Tarifa, valorTarifa);
            }

            this.ContadorDeSaques++;
        }

        public override void AplicarFechamentoMensal()
        {
            this.ContadorDeSaques = 0;
        }
    }
}