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
            var sql = @"SELECT DISTINCT c.Id, c.UsuarioId, c.DataCadastro, c.DataAtualizacao, cc.Status  FROM Colaborador c
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id 
                        WHERE cc.ContaId  = @ContaId";
            var retorno = await _dbAgenda.Connection.QueryAsync<ColaboradorDTO>(sql, new { ContaId = contaId }) ?? Enumerable.Empty<ColaboradorDTO>();
            return retorno;
        }

        public async Task<int> Salvar(ContaColaborador colaborador)
        {
            var sql = @"INSERT INTO ContaColaborador (ColaboradorId, ContaId,DataCadastro, Status) VALUES (@ColaboradorId, @ContaId, @DataCadastro,@Status);
                        SELECT LAST_INSERT_ID();";
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                ColaboradorId = colaborador.ColaboradorId,
                ContaId = colaborador.ContaId,
                DataCadastro = colaborador.DataCadastro,
                Status = colaborador.Status
            }, _dbAgenda.Transaction);
            return retorno;
        }
        public async Task<bool> Atualizar(ContaColaborador colaborador)
        {
            var sql = @"
                        UPDATE ContaColaborador
                        SET 
                            ContaId = @ContaId,
                            DataCadastro = @DataCadastro,
                            Status = @Status
                        WHERE 
                            ColaboradorId = @ColaboradorId;";

            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                ColaboradorId = colaborador.ColaboradorId,
                ContaId = colaborador.ContaId,
                DataCadastro = colaborador.DataCadastro,
                Status = colaborador.Status
            }, _dbAgenda.Transaction);

            return retorno > 0;
        }

    }
}
