using System;
using MiniBanco.ConsoleApp.Exceptions;
using MiniBanco.ConsoleApp.Models.Enums;

namespace MiniBanco.ConsoleApp.Models
{
    public class Transacao
    {
        public string Descricao { get; private set; }
        public TipoTransacao Tipo { get; private set; }
        public decimal Valor { get; private set; }

        public Transacao(string descricao, TipoTransacao tipo, decimal valor)
        {
            if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new EntradaInvalidaException("A descrição da transação não pode ser vazia.");
            }

            if (valor <= 0)
            {
                throw new ValorInvalidoException("O valor da transação deve ser maior que zero.");
            }

            this.Descricao = descricao;
            this.Tipo = tipo;
            this.Valor = valor;
        }
    }
}