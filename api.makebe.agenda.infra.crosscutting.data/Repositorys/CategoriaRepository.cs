using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly DbAgenda _dbAgenda;

        public CategoriaRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<int> Salvar(Categoria categoria)
        {
            var sql = @"INSERT INTO Categoria (IdServico, Descricao, DataCadastro, Ativo, CategoriaItemId)
                        VALUES (@ServicoId, @Descricao, @DataCadastro, @Ativo, @CategoriaItemId);
                        SELECT LAST_INSERT_ID() AS LastInsertedId;";

            return await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                categoria.ServicoId,
                categoria.Descricao,
                categoria.DataCadastro,
                categoria.Ativo,
                categoria.CategoriaItemId
            }, _dbAgenda.Transaction);
        }

        public async Task<IEnumerable<Categoria>> BuscarPorServico(int servicoId)
        {
            var sql = @"SELECT Id, IdServico, Descricao, DataCadastro, Ativo
                        FROM Categoria
                        WHERE IdServico = @ServicoId
                          AND Ativo = 1";

            return await _dbAgenda.Connection.QueryAsync<Categoria>(sql, new { ServicoId = servicoId }, _dbAgenda.Transaction)
                ?? Enumerable.Empty<Categoria>();
        }

        public async Task<bool> DesativarPorServico(int servicoId)
        {
            var sql = @"UPDATE Categoria
                        SET Ativo = 0
                        WHERE IdServico = @ServicoId
                          AND Ativo = 1";

            var response = await _dbAgenda.Connection.ExecuteAsync(sql, new { ServicoId = servicoId }, _dbAgenda.Transaction);
            return response > 0;
        }
    }
}
