using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class MovimentoQueryStore
    {
        private readonly DatabaseConfig _config;

        public MovimentoQueryStore(DatabaseConfig config)
        {
            _config = config;
        }

        public decimal ObterTotalMovimentos(string idContaCorrente, string tipo)
        {
            using var connection = new SqliteConnection(_config.Name);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT COALESCE(SUM(valor), 0) FROM movimento
                WHERE idcontacorrente = @id AND tipomovimento = @tipo;";

            cmd.Parameters.AddWithValue("@id", idContaCorrente);
            cmd.Parameters.AddWithValue("@tipo", tipo);

            var result = cmd.ExecuteScalar();
            return Convert.ToDecimal(result);
        }
    }
}
