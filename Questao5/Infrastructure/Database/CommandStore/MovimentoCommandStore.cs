using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class MovimentoCommandStore
    {
        private readonly DatabaseConfig _config;

        public MovimentoCommandStore(DatabaseConfig config)
        {
            _config = config;
        }

        public void RegistrarMovimento(string idMovimento, string idConta, DateTime data, string tipo, decimal valor)
        {
            //using var connection = new SqliteConnection(_config.ConnectionString);
            using var connection = new SqliteConnection(_config.Name);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                VALUES (@id, @idconta, @data, @tipo, @valor);";

            cmd.Parameters.AddWithValue("@id", idMovimento);
            cmd.Parameters.AddWithValue("@idconta", idConta);
            cmd.Parameters.AddWithValue("@data", data.ToString("dd/MM/yyyy"));
            cmd.Parameters.AddWithValue("@tipo", tipo);
            cmd.Parameters.AddWithValue("@valor", valor);

            cmd.ExecuteNonQuery();
        }
    }
}
