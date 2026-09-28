using System.Net.Http.Headers;
using System.Text.Json;
using Transacao.API.Domain.DTOs;

namespace Transacao.API.Infrastructure.Clients
{
    public class ClienteClient : IClienteClient
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public ClienteClient(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
        }

        public async Task<ClienteDto?> ObterClienteAsync(Guid Id, string token)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Clientes/{Id}");
            
            if (!string.IsNullOrWhiteSpace(token))            
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _http.SendAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(jsonResponse))
                return null;
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
                          
            var dto = JsonSerializer.Deserialize<ClienteDto>(jsonResponse, options);
            return dto;
        }

        public async Task<bool> AtualizarSaldoAsync(Guid id, decimal valor, string token)
        {
            var payload = new { Id = id, Valor = valor };
            using var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/Clientes/atualizar-saldo?Id={id}&valor={valor}");
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            request.Content = JsonContent.Create(payload);

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
