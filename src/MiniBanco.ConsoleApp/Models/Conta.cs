using System;
using System.Collections.Generic;
using MiniBanco.ConsoleApp.Exceptions;
using MiniBanco.ConsoleApp.Models.Enums;

namespace MiniBanco.ConsoleApp.Models
{
    public abstract class Conta
    {
        public string Titular { get; private set; }
        public int Numero { get; private set; }
        public decimal Saldo { get; protected set; }
        public List<Transacao> Extrato { get; private set; }

        public Conta(string titular, int numero, decimal saldoInicial)
        {
            this.Titular = titular;
            this.Numero = numero;
            this.Saldo = saldoInicial;
            this.Extrato = new List<Transacao>();
        }

        public void RegistrarTransacao(string descricao, TipoTransacao tipo, decimal valor)
        {
            var transacao = new Transacao(descricao, tipo, valor);
            this.Extrato.Add(transacao);
        }

        public void Depositar(decimal valor, string descricao)
        {
            if (valor <= 0)
            {
                throw new ValorInvalidoException("O valor do depósito deve ser maior que zero.");
            }

            this.Saldo += valor;
            RegistrarTransacao(descricao, TipoTransacao.Deposito, valor);
        }

        public virtual void Sacar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ValorInvalidoException("O valor deve ser maior que zero.");
            }

            if (valor > this.Saldo)
            {
                throw new SaldoInsuficienteException("Saldo Insuficiente");
            }

            this.Saldo -= valor;
            RegistrarTransacao("Saque", TipoTransacao.Saque, valor);
        }

        public virtual void AplicarFechamentoMensal() { }

    }
}