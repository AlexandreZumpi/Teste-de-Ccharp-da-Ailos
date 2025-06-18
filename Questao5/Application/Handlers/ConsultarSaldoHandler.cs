using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoHandler
    {
        private readonly ContaCorrenteQueryStore _contaStore;
        private readonly MovimentoQueryStore _movimentoStore;

        public ConsultarSaldoHandler(ContaCorrenteQueryStore contaStore, MovimentoQueryStore movimentoStore)
        {
            _contaStore = contaStore;
            _movimentoStore = movimentoStore;
        }

        public ConsultarSaldoResponse Handle(ConsultarSaldoQuery query)
        {
            var conta = _contaStore.ObterPorNumero(query.NumeroConta);
            if (conta == null) throw new BusinessException("INVALID_ACCOUNT", "Conta inexistente.");
            if (!conta.Ativo) throw new BusinessException("INACTIVE_ACCOUNT", "Conta inativa.");

            var creditos = _movimentoStore.ObterTotalMovimentos(conta.IdContaCorrente, "C");
            var debitos = _movimentoStore.ObterTotalMovimentos(conta.IdContaCorrente, "D");

            return new ConsultarSaldoResponse
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                DataHoraConsulta = DateTime.Now,
                SaldoAtual = creditos - debitos
            };
        }
    }
}
