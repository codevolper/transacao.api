namespace Transacao.API.Domain.Interfaces
{
    public interface ITransacaoRepository
    {
        Task SalvarAsync(Transacao.API.Domain.Entidades.Transacao transacao);

        Task<Transacao.API.Domain.Entidades.Transacao?> ObterPorNumeroAsync(string numeroTransacao);
    }
}
