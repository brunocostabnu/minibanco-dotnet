using MiniBanco.ConsoleApp.Models;
using MiniBanco.ConsoleApp.Models.Enums;

namespace MiniBanco.ConsoleApp.Services
{
    public interface IMotorAnalise
    {
        (int calculoPontuacao, int quantidadeTransacao) CalcularPontuacao(Conta conta);
        double CalcularIndiceSaude(Conta conta);
        ClassificacaoConta ClassificarConta(Conta conta);
    }
}