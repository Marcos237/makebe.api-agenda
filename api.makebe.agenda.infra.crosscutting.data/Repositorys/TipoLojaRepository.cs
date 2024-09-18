using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class TipoLojaRepository : ITipoLojaRepository
    {
        private DbAgenda _dbAgenda;
        public TipoLojaRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;   
        }
        public async Task<IEnumerable<TipoLoja>> BuscarTodos()
        {
            var sql = @"SELECT * FROM TipoLoja";
            var result = await _dbAgenda.Connection.QueryAsync<TipoLoja>(sql);

            return result;
        }
    }
}
