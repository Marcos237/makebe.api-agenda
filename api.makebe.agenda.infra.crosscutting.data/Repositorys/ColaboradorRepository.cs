using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly DbAgenda _dbAgenda;
        public ColaboradorRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<LojaColaboradorDTO> BuscarPorId(int id)
        {
            var sql = @"SELECT c.*, lc.Id AS LojaColaboradorId FROM LojaColaborador lc 
                        INNER JOIN Colaborador c  ON lc.ColaboradorId  = c.Id 
                        WHERE c.Id  = @Id";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<LojaColaboradorDTO>(sql, new { Id = id }) ?? new LojaColaboradorDTO();
            return retorno;
        }
        public async Task<int> Salvar(Colaborador colaborador)
        {
            var sql = @"                      
                        INSERT INTO Colaborador (UsuarioId, DataCadastro, DataAtualizacao, Status) VALUES  (@UsuarioId, @DataCadastro, @DataAtualizacao, @Status);                     
                        SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new { colaborador }, _dbAgenda.Transaction);
            return retorno;
        }
        public async Task<Colaborador> Atualizar(Colaborador colaborador)
        {
            var sql = @"                      
                        UPDATE Colaborador SET 
                             DataAtualizacao  = @DataAtualizacao,
                             Status  = @Status
                        WHERE Id = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new { colaborador }, _dbAgenda.Transaction);
            return colaborador;
        }
        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE Colaborador SET 
                      Status = false 
                      Where Id = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            { Id = id }) > 0;
            return retorno;
        }
    }
}
