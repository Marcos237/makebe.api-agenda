using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class PermissaoPapelRepository : IPermissaoPapelRepository
    {
        private readonly DbAgenda _dbAgenda;

        public PermissaoPapelRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<PermissaoPapel?> BuscarPorPermissaoId(Guid permissaoId)
        {
            const string sql = @"SELECT
                                    Id,
                                    Descricao,
                                    PapeisId,
                                    Papeis
                                FROM vw_permissao_papeis
                                WHERE Id = @PermissaoId";

            return await _dbAgenda.Connection.QueryFirstOrDefaultAsync<PermissaoPapel>(sql, new { PermissaoId = permissaoId });
        }
    }
}
