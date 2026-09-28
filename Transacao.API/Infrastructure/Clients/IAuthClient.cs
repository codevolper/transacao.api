namespace Transacao.API.Infrastructure.Clients
{
    public interface IAuthClient
    {
        Task<bool> ValidarTokenAsync(string token);
    }
}
