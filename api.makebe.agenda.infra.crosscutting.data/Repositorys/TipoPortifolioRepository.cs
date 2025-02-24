using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class TipoPortifolioRepository : ITipoPortifolioRepository
    {
        private readonly DbAgenda _dbAgenda;
        public TipoPortifolioRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<IEnumerable<TipoPortifolioDTO>> BuscarPorTipoUsuarioId(int tipoPortifolioId)
        {
            var sql = @"SELECT tp.Id, tp.TipoUsuarioId, tp.Descricao, Label, tup.Descricao AS NomeTipo, Titulo FROM TipoPortifolio tp 
                        INNER JOIN TipoUsuario tup ON tup.Id = tp.TipoUsuarioId 
                        WHERE tp.TipoUsuarioId  = @TipoUsuarioId
                        AND tp.Status  = 1";
            var response = await _dbAgenda.Connection.QueryAsync<TipoPortifolioDTO>(sql, new { TipoUsuarioId = tipoPortifolioId }) 
                ?? Enumerable.Empty<TipoPortifolioDTO>();
            return response;
        }
    }
}
