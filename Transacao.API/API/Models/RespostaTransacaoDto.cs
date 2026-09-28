using System.Text.Json.Serialization;

namespace Transacao.API.API.Models
{
    public class RespostaTransacaoDto
    {
        [JsonIgnore]
        public bool Sucesso { get; set; }

        [JsonPropertyName("status")]
        public string Mensagem { get; set; } = string.Empty;

        [JsonPropertyName("idTransacao")]
        public string? NumeroTransacao { get; set; }

        [JsonIgnore]
        public decimal? Valor { get; set; }
    }
}
