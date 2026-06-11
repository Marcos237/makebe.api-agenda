using api.makebe.agenda.domain.Constants;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class DbManutencao : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbManutencao(IConfiguration configuration)
        {
            _configuration = configuration;
            string _connectionString = _configuration.GetConnectionString(DataBaseConstant.ManutencaoBase) ?? string.Empty;
            Connection = new MySqlConnection(_connectionString);
            Connection?.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
