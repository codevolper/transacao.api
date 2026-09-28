using System.Data;
using Transacao.API.Domain.Interfaces;

namespace Transacao.API.Infrastructure.Persistence
{
    // Implementação de repositório que usa DataTable em memória.
    public class InMemoryTransacaoRepository : ITransacaoRepository
    {
        public Task SalvarAsync(Transacao.API.Domain.Entidades.Transacao transacao)
        {
            var tabela = InMemoryDataStore.Transacoes;

            var linha = tabela.NewRow();            
            linha["ClienteId"] = transacao.ClienteId;
            linha["Valor"] = transacao.Valor;            
            linha["NumeroTransacao"] = transacao.NumeroTransacao;
            tabela.Rows.Add(linha);

            return Task.CompletedTask;
        }

        public Task<Transacao.API.Domain.Entidades.Transacao?> ObterPorNumeroAsync(string numeroTransacao)
        {
            var tabela = InMemoryDataStore.Transacoes;
            foreach (DataRow row in tabela.Rows)
            {
                if (row["NumeroTransacao"].ToString() == numeroTransacao)
                {
                    var t = new Transacao.API.Domain.Entidades.Transacao
                    {                        
                        ClienteId = (Guid)row["ClienteId"],
                        Valor = Convert.ToDecimal(row["Valor"]),                     
                        NumeroTransacao = (Guid)row["NumeroTransacao"]
                    };
                    return Task.FromResult<Transacao.API.Domain.Entidades.Transacao?>(t);
                }
            }

            return Task.FromResult<Transacao.API.Domain.Entidades.Transacao?>(null);
        }
    }
}
