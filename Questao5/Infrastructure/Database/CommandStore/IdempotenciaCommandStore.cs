using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class IdempotenciaCommandStore
    {
        private readonly DatabaseConfig _config;

        public IdempotenciaCommandStore(DatabaseConfig config)
        {
            _config = config;
        }

        public bool ExisteChave(string chave, out string resultado)
        {
            resultado = string.Empty;

            using var connection = new SqliteConnection(_config.Name);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @chave";
            cmd.Parameters.AddWithValue("@chave", chave);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                resultado = reader.GetString(0);
                return true;
            }
            return false;
        }

        public void Salvar(string chave, MovimentarContaCommand requisicao, string resultado)
        {
            using var connection = new SqliteConnection(_config.Name);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                VALUES (@chave, @requisicao, @resultado);";

            cmd.Parameters.AddWithValue("@chave", chave);
            cmd.Parameters.AddWithValue("@requisicao", JsonConvert.SerializeObject(requisicao));
            cmd.Parameters.AddWithValue("@resultado", resultado);

            cmd.ExecuteNonQuery();
        }
    }
}
