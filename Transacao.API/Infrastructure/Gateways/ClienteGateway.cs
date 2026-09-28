using System.Threading.Tasks;
using Transacao.API.Domain.DTOs;
using Transacao.API.Infrastructure.Clients;

namespace Transacao.API.Infrastructure.Gateways
{
    public class ClienteGateway : IClienteGateway
    {
        private readonly IClienteClient _clienteClient;

        public ClienteGateway(IClienteClient clienteClient)
        {
            _clienteClient = clienteClient;
        }

        public Task<ClienteDto?> ObterClienteAsync(Guid id, string token)
        {
            return _clienteClient.ObterClienteAsync(id, token);
        }

        public Task<bool> AtualizarSaldoAsync(Guid id, decimal valor, string token)
        {
            return _clienteClient.AtualizarSaldoAsync(id, valor, token);
        }
    }
}
