using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class CategoriaItemRepository : ICategoriaItemRepository
    {
        private readonly DbAgenda _dbAgenda;

        public CategoriaItemRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<IEnumerable<CategoriaItem>> BuscarTodosAtivos()
        {
            var sql = @"SELECT Id, Descricao, DataCadastro, Status
                        FROM CategoriaItem
                        WHERE Status = 1
                        ORDER BY Descricao";

            return await _dbAgenda.Connection.QueryAsync<CategoriaItem>(sql)
                ?? Enumerable.Empty<CategoriaItem>();
        }

        public async Task<CategoriaItem?> BuscarPorId(int id)
        {
            var sql = @"SELECT Id, Descricao, DataCadastro, Status
                        FROM CategoriaItem
                        WHERE Id = @Id
                          AND Status = 1";

            return await _dbAgenda.Connection.QueryFirstOrDefaultAsync<CategoriaItem>(sql, new { Id = id });
        }
    }
}
