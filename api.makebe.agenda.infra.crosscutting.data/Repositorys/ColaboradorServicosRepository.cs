using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ColaboradorServicosRepository : IColaboradorServicosRepository
    {
        private readonly DbAgenda _dbAgenda;

        public ColaboradorServicosRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<int> Salvar(ColaboradorServicos colaboradorServico)
        {
            var sql = @"INSERT INTO ColaboradorServicos (IdColaborador, IdServico, DataCadastro)
                        VALUES (@IdColaborador, @IdServico, @DataCadastro);
                        SELECT LAST_INSERT_ID();";

            return await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                colaboradorServico.IdColaborador,
                colaboradorServico.IdServico,
                colaboradorServico.DataCadastro
            }, _dbAgenda.Transaction);
        }

        public async Task<ColaboradorServicos> BuscarPorId(int id)
        {
            var sql = @"SELECT Id, IdColaborador, IdServico, DataCadastro
                        FROM ColaboradorServicos
                        WHERE Id = @Id";

            return await _dbAgenda.Connection.QueryFirstOrDefaultAsync<ColaboradorServicos>(
                sql,
                new { Id = id },
                _dbAgenda.Transaction) ?? new ColaboradorServicos();
        }

        public async Task<IEnumerable<ColaboradorServicos>> BuscarPorColaboradorId(int colaboradorId)
        {
            var sql = @"SELECT Id, IdColaborador, IdServico, DataCadastro
                        FROM ColaboradorServicos
                        WHERE IdColaborador = @IdColaborador";

            return await _dbAgenda.Connection.QueryAsync<ColaboradorServicos>(
                sql,
                new { IdColaborador = colaboradorId },
                _dbAgenda.Transaction) ?? Enumerable.Empty<ColaboradorServicos>();
        }

        public async Task<bool> Atualizar(ColaboradorServicos colaboradorServico)
        {
            var sql = @"UPDATE ColaboradorServicos
                        SET IdColaborador = @IdColaborador,
                            IdServico = @IdServico,
                            DataCadastro = @DataCadastro
                        WHERE Id = @Id;";

            return await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                colaboradorServico.Id,
                colaboradorServico.IdColaborador,
                colaboradorServico.IdServico,
                colaboradorServico.DataCadastro
            }, _dbAgenda.Transaction) > 0;
        }

        public async Task<bool> Remover(int id)
        {
            var sql = @"DELETE FROM ColaboradorServicos
                        WHERE Id = @Id;";

            return await _dbAgenda.Connection.ExecuteAsync(sql, new { Id = id }, _dbAgenda.Transaction) > 0;
        }

        public async Task<bool> RemoverPorColaboradorEServico(int colaboradorId, int servicoId)
        {
            var sql = @"DELETE FROM ColaboradorServicos
                        WHERE IdColaborador = @IdColaborador
                          AND IdServico = @IdServico;";

            return await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                IdColaborador = colaboradorId,
                IdServico = servicoId
            }, _dbAgenda.Transaction) > 0;
        }

        public async Task<bool> RemoverTodosPorColaborador(int colaboradorId)
        {
            var sql = @"DELETE FROM ColaboradorServicos
                        WHERE IdColaborador = @IdColaborador;";

            await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                IdColaborador = colaboradorId
            }, _dbAgenda.Transaction);

            return true;
        }
    }
}
