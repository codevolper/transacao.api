using System.Net.Http.Headers;

namespace Transacao.API.Infrastructure.Clients
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _http;

        public AuthClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> ValidarTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;

            using var request = new HttpRequestMessage(HttpMethod.Get, "api/Usuarios/validar-token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
