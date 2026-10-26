using MiniBanco.ConsoleApp.Models;
using MiniBanco.ConsoleApp.Models.Enums;

namespace MiniBanco.ConsoleApp.Services
{
    public interface IRelatorioService
    {
        void ExibirRelatorioContas(List<Conta> contas);
        void DadosDaConta(Conta conta);

        void MovimentacoesRegistradas(Conta conta);

        void ResumoPontuacao(Conta conta, IMotorAnalise motorAnalise);

        void RelatorioPendenciaRecomendacaoGerencia(Conta conta);
    }
}