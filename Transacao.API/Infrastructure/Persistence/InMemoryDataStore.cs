using System.Data;

namespace Transacao.API.Infrastructure.Persistence
{
    // Armazenamento em memória usando DataSet/DataTable
    public static class InMemoryDataStore
    {
        private static readonly DataSet _dataSet;

        static InMemoryDataStore()
        {
            _dataSet = new DataSet("TransacaoDataSet");

            var tabelaTransacoes = new DataTable("Transacoes");

            var colunaId = new DataColumn("Id", typeof(int))
            {
                AutoIncrement = true,      
                AutoIncrementSeed = 1,     
                AutoIncrementStep = 1      
            };

            tabelaTransacoes.Columns.Add(colunaId);
            tabelaTransacoes.Columns.Add("ClienteId", typeof(Guid));
            tabelaTransacoes.Columns.Add("Valor", typeof(decimal));
            tabelaTransacoes.Columns.Add("Data", typeof(DateTime));
            tabelaTransacoes.Columns.Add("NumeroTransacao", typeof(Guid));

            tabelaTransacoes.PrimaryKey = new DataColumn[] { tabelaTransacoes.Columns["Id"] };

            _dataSet.Tables.Add(tabelaTransacoes);
        }

        public static DataTable Transacoes => _dataSet.Tables["Transacoes"];
    }
}
