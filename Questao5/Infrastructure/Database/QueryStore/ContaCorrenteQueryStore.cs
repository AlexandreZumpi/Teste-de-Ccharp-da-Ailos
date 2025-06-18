using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class ContaCorrenteQueryStore
    {
        private readonly DatabaseConfig _config;

        public ContaCorrenteQueryStore(DatabaseConfig config)
        {
            _config = config;
        }

        public ContaCorrente? ObterPorNumero(int numero)
        {
            using var connection = new SqliteConnection(_config.Name);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT idcontacorrente, numero, nome, ativo FROM contacorrente WHERE numero = @numero LIMIT 1";
            cmd.Parameters.AddWithValue("@numero", numero);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ContaCorrente
                {
                    IdContaCorrente = reader.GetString(0),
                    Numero = reader.GetInt32(1),
                    Nome = reader.GetString(2),
                    Ativo = reader.GetInt32(3) == 1
                };
            }
            return null;
        }
    }
}
