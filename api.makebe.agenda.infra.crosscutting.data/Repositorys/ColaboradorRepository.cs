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

        public async Task<ColaboradorDTO> BuscarPorUsuarioId(Guid id)
        {
            var sql = @"SELECT c.Id, c.UsuarioId , c.DataCadastro , c.DataAtualizacao ,c.Status  FROM ContaColaborador cc 
                        INNER JOIN Colaborador c  ON cc.ColaboradorId  = c.Id 
                        WHERE c.UsuarioId  = @UsuarioId";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<ColaboradorDTO>(sql, new { UsuarioId = id }) ?? new ColaboradorDTO();
            return retorno;
        }
        public async Task<ColaboradorDTO> BuscarPorId(int id)
        {
            var sql = @"SELECT * FROM Colaborador WHERE c.Id = @Id";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<ColaboradorDTO>(sql, new { Id = id }) ?? new ColaboradorDTO();
            return retorno;
        }

        public async Task<int> Salvar(Colaborador colaborador)
        {
            var sql = @"                      
                        INSERT INTO Colaborador (UsuarioId, DataCadastro, DataAtualizacao, Status) VALUES  (@UsuarioId, @DataCadastro, @DataAtualizacao, @Status);                     
                        SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                UsuarioId = colaborador.UsuarioId.ToString(),
                DataCadastro = colaborador.Datacadastro,
                DataAtualizacao = colaborador.DataAtualizacao,
                Status = colaborador.Status
            }, _dbAgenda.Transaction);
            return retorno;

        }
        public async Task<Colaborador> Atualizar(Colaborador colaborador)
        {
            var sql = @"                      
                        UPDATE Colaborador SET 
                             DataAtualizacao  = @DataAtualizacao,
                             Status  = @Status
                        WHERE Id = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new {
                DataAtualizacao = colaborador.DataAtualizacao,
                Status = colaborador.Status,
                Id = colaborador.Id
            }, _dbAgenda.Transaction);
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
