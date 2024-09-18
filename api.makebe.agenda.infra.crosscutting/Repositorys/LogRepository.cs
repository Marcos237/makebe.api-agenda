using api.makebe.agenda.infra.crosscutting.Entidades;
using api.makebe.agenda.infra.crosscutting.Repositorys.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace api.makebe.agenda.infra.crosscutting.Repositorys
{
    public class LogRepository : ILogRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _conection;
        public LogRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conection = _configuration.GetConnectionString("LogConnection") ?? string.Empty;
        }
        public async Task<bool> Gravarlog(Log log)
        {
            using IDbConnection db = new MySqlConnection(_conection);

            string sql = @"INSERT INTO Log (Metodo, Mensagem, Objeto, DataCadastro, Usuario, Request, CamposValidados, Tipo)
                       VALUES (@Metodo, @Mensagem, @Objeto, @DataCadastro, @Usuario, @Request, @CamposValidados, @Tipo)";

            return await db.ExecuteAsync(sql, log) > 0;
        }
    }
}
