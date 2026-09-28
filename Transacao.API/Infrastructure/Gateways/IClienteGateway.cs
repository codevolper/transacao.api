using System.Threading.Tasks;
using Transacao.API.Domain.DTOs;

namespace Transacao.API.Infrastructure.Gateways
{
    public interface IClienteGateway
    {
        Task<ClienteDto?> ObterClienteAsync(Guid id, string token);

        Task<bool> AtualizarSaldoAsync(Guid id, decimal valor, string token);
    }
}
