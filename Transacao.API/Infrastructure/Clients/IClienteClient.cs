using Transacao.API.Domain.DTOs;

namespace Transacao.API.Infrastructure.Clients
{
    public interface IClienteClient
    {
        Task<ClienteDto?> ObterClienteAsync(Guid id, string token);

        Task<bool> AtualizarSaldoAsync(Guid id, decimal valor, string token);
    }
}
