using Transacao.API.API.Models;

namespace Transacao.API.Application.UseCases
{
    public interface IProcessarPagamentoUseCase
    {
        Task<RespostaTransacaoDto> ProcessarAsync(RequisicaoPagamentoDto requisicao, string token);
    }
}
