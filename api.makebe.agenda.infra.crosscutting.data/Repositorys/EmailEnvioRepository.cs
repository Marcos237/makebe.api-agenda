using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class EmailEnvioRepository : IEmailEnvioRepository
    {
        private readonly DbManutencao _dbManutencao;

        public EmailEnvioRepository(DbManutencao dbManutencao)
        {
            _dbManutencao = dbManutencao;
        }

        public async Task<int> Salvar(EmailEnvio emailEnvio)
        {

            var sql = @"
                INSERT INTO EmailEnvio
                    (DadosModelo, DadosEnvio, Pasta, NomeArquivo, DataCadastro, Processado, Tentativas, DataProcessamento, Erro)
                VALUES
                    (@DadosModelo, @DadosEnvio, @Pasta, @NomeArquivo, @DataCadastro, @Processado, @Tentativas, @DataProcessamento, @Erro);
                SELECT LAST_INSERT_ID();";

            return await _dbManutencao.Connection.ExecuteScalarAsync<int>(sql, new
            {
                emailEnvio.DadosModelo,
                emailEnvio.DadosEnvio,
                emailEnvio.Pasta,
                emailEnvio.NomeArquivo,
                emailEnvio.DataCadastro,
                emailEnvio.Processado,
                emailEnvio.Tentativas,
                emailEnvio.DataProcessamento,
                emailEnvio.Erro
            }, _dbManutencao.Transaction);

        }
    }
}
