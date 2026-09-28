namespace Transacao.API.Domain.Entidades
{
    public class Transacao
    {
        public Guid ClienteId { get; set; }

        public decimal Valor { get; set; }        

        public Guid NumeroTransacao { get; set; } 
    }
}
