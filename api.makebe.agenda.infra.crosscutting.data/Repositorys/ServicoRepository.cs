using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ServicoRepository : IServicosRepository
    {
        private readonly DbAgenda _dbAgenda;
        public ServicoRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<IEnumerable<Servicos>> BuscarServicos()
        {
            var sql = @"SELECT ID, Descricao FROM  Servicos WHERE Status = 1";
            var retorno = await _dbAgenda.Connection.QueryAsync<Servicos>(sql) ?? Enumerable.Empty<Servicos>();
            return retorno;
        }
    }
}
