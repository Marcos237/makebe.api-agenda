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
        public async Task<IEnumerable<TipoPortifolioDTO>> BuscarPorTipoUsuarioPortifolioId(int tipoPortifolioId)
        {
            var sql = @"SELECT tp.Id, tp.TipoUsuarioPortifolioId, tp.Descricao, Label, tup.Descricao AS NomeTipo, Titulo FROM TipoPortifolio tp 
                        INNER JOIN TipoUsuarioPortifolio tup ON tup.Id = tp.TipoUsuarioPortifolioId 
                        WHERE tp.TipoUsuarioPortifolioId  = @TipoUsuarioPortifolioId
                        AND tp.Status  = 1";
            var response = await _dbAgenda.Connection.QueryAsync<TipoPortifolioDTO>(sql, new { TipoUsuarioPortifolioId = tipoPortifolioId }) 
                ?? Enumerable.Empty<TipoPortifolioDTO>();
            return response;
        }
    }
}
