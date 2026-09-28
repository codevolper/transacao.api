namespace Transacao.API.Infrastructure.Auth
{
    public interface IValidadorTokenRemoto
    {
        Task<bool> ValidarAsync(string token);
    }
}
