using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ContaColaboradorRepository : IContaColaboradorRepository
    {
        private readonly DbAgenda _dbAgenda;
        public ContaColaboradorRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<IEnumerable<ColaboradorDTO>> BuscarColaboradorPorContaId(string contaId)
        {
            var sql = @"SELECT DISTINCT c.* FROM ContaColaborador uc 
                        INNER JOIN Colaborador c ON c.Id  = uc.ColaboradorId 
                        WHERE uc.ContaId  = @ContaId";
            var retorno = await _dbAgenda.Connection.QueryAsync<ColaboradorDTO>(sql, new { ContaId = contaId }) ?? Enumerable.Empty<ColaboradorDTO>();
            return retorno;
        }

        public async Task<int> Salvar(ContaColaborador colaborador)
        {
            var sql = @"INSERT INTO ContaColaborador (ColaboradorId, ContaId,DataCadastro) VALUES (@ColaboradorId, @ContaId, @DataCadastro);
                        SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                ColaboradorId = colaborador.ColaboradorId,
                ContaId = colaborador.ContaId,
                DataCadastro = colaborador.DataCadastro
            }, _dbAgenda.Transaction);
            return retorno;
        }
    }
}
