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
        public async Task<IEnumerable<Servicos>> BuscarServicos(string contaId)
        {
            var sql = @"SELECT s.ID, s.Descricao FROM  Servicos s
                        INNER JOIN ContaServico cs  ON cs.ServicoId  = s.Id 
                        WHERE ContaId = @ContaId  AND s.Status = 1";
            var retorno = await _dbAgenda.Connection.QueryAsync<Servicos>(sql, new {ContaId = contaId}) ?? Enumerable.Empty<Servicos>();
            return retorno;
        }
    }
}
