using System.Net.Http.Json;

namespace Transacao.API.Infrastructure.Auth
{
    public class ValidadorTokenRemoto : IValidadorTokenRemoto
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ValidadorTokenRemoto(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<bool> ValidarAsync(string token)
        {
            var baseUrl = _configuration["AuthApi:BaseUrl"] ?? "https://localhost:60285";
            var client = _httpClientFactory.CreateClient("AuthApi");
            client.BaseAddress = new Uri(baseUrl);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Usuarios/validar-token");
            request.Content = JsonContent.Create(new { token = token });

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;

            return false;           
        }

        private class ValidationResult
        {
            public bool IsValid { get; set; }
        }
    }
}
