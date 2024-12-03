using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class LojaColaboradorRepository : ILojaColaboradorRepository
    {
        private readonly DbAgenda _dbAgenda;
        public LojaColaboradorRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<IEnumerable<LojaColaboradorDTO>> BuscarColaboradorPorLoja(int lojaId)
        {
            var sql = @"SELECT c.*, lc.Id AS LojaColaboradorId FROM LojaColaborador lc 
                        INNER JOIN Colaborador c  ON lc.ColaboradorId  = c.Id 
                        WHERE lc.LojaId  = @LojaId";
            var retorno = await _dbAgenda.Connection.QueryAsync<LojaColaboradorDTO>(sql, new { LojaId = lojaId }) ?? Enumerable.Empty<LojaColaboradorDTO>();
            return retorno;
        }
        public async Task<int> Salvar(LojaColaborador colaborador)
        {
            var sql = @"                      
                        INSERT INTO LojaColaborador (LojaId, ColaboradorId, DataCadastro) VALUES(@LojaId, @ColaboradorId, @DataCadastro);                     
                        SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new { colaborador }, _dbAgenda.Transaction);
            return retorno;
        }
        public async Task<LojaColaborador> Atualizar(LojaColaborador colaborador)
        {
            var sql = @"                      
                        UPDATE LojaColaborador  
                        SET LojaId  = @LojaId,
                        ColaboradorId  = @ColaboradorId
                        WHERE Id  = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new { colaborador }, _dbAgenda.Transaction);
            return colaborador;
        }
    }
}
