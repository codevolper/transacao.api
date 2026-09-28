using System.Text.Json.Serialization;

namespace Transacao.API.API.Models
{
    public class RequisicaoPagamentoDto
    {
        [JsonPropertyName("idCliente")]
        public Guid Id { get; set; }

        [JsonPropertyName("valorSimulacao")]
        public decimal Valor { get; set; }
    }
}
