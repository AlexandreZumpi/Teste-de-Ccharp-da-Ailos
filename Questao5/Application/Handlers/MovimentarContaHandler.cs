using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaHandler
    {
        private readonly ContaCorrenteQueryStore _contaStore;
        private readonly MovimentoCommandStore _movimentoStore;
        private readonly IdempotenciaCommandStore _idempotenciaStore;

        public MovimentarContaHandler(
            ContaCorrenteQueryStore contaStore,
            MovimentoCommandStore movimentoStore,
            IdempotenciaCommandStore idempotenciaStore)
        {
            _contaStore = contaStore;
            _movimentoStore = movimentoStore;
            _idempotenciaStore = idempotenciaStore;
        }

        public MovimentarContaResponse Handle(MovimentarContaCommand request)
        {
            var conta = _contaStore.ObterPorNumero(request.NumeroConta);
            if (conta == null) throw new BusinessException("INVALID_ACCOUNT", "Conta inexistente.");
            if (!conta.Ativo) throw new BusinessException("INACTIVE_ACCOUNT", "Conta inativa.");
            if (request.Valor <= 0) throw new BusinessException("INVALID_VALUE", "Valor deve ser positivo.");
            if (!TipoMovimento.IsValid(request.TipoMovimento)) throw new BusinessException("INVALID_TYPE", "Tipo de movimento deve ser 'C' ou 'D'.");

            var movimentoId = Guid.NewGuid().ToString();
            _movimentoStore.RegistrarMovimento(movimentoId, conta.IdContaCorrente, DateTime.Now, request.TipoMovimento.ToUpper(), request.Valor);
            _idempotenciaStore.Salvar(Guid.NewGuid().ToString(), request, movimentoId);

            return new MovimentarContaResponse { IdMovimento = movimentoId };
        }
    }
}
