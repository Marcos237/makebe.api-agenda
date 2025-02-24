using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ColaboradorEnderecoRepository : IEnderecoContextRepository<ColaboradorEndereco, EnderecoDTO>
    {
        private readonly DbAgenda _dbAgenda;
        public ColaboradorEnderecoRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<IEnumerable<EnderecoDTO>> BuscarEnderecos(string contaId)
        {
            var sql = @"SELECT e.*, ce.Id as ColaboradorEnderecoId, CAST(cc.ContaId AS CHAR) AS ContaId, CAST(c.UsuarioId AS CHAR) AS UsuarioId FROM Endereco e
                            INNER JOIN ColaboradorEndereco ce ON ce.EnderecoId = e.Id 
                            INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = ce.ColaboradorId 
                            INNER JOIN Colaborador c ON c.Id  = cc.ColaboradorId   
                            WHERE  cc.ContaId = @ContaId
                            AND e.Status = 1";
            var response = await _dbAgenda.Connection.QueryAsync<EnderecoDTO>(sql, new { ContaId = contaId });
            return response;
        }

        public async Task<int> Salvar(ColaboradorEndereco item)
        {
            var sql = @"INSERT INTO ColaboradorEndereco (ColaboradorId, EnderecoId, DataCadastro, Status) VALUES (@ColaboradorId, @EnderecoId, @DataCadastro, @Status);
                      SELECT LAST_INSERT_ID();";

            var parametros = new
            {
                ColaboradorId = item.ColaboradorId,
                EnderecoId = item.EnderecoId,
                DataCadastro = item.DataCadastro,
                Status = item.Status
            };
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, parametros, _dbAgenda.Transaction);
            return retorno;
        }
        public async Task<bool> Atualizar(ColaboradorEndereco item)
        {
            var sql = @"UPDATE ColaboradorEndereco SET
                         ColaboradorId  = @ColaboradorId,						
                         EnderecoId = @EnderecoId,
                         DataCadastro  = @DataCadastro
                        WHERE Id = @Id";
            var parametros = new
            {
                ColaboradorId = item.ColaboradorId,
                EnderecoId = item.EnderecoId,
                DataCadastro = item.DataCadastro,
                Id = item.Id

            };
            var response = await _dbAgenda.Connection.ExecuteAsync(sql, parametros, _dbAgenda.Transaction) > 0;
            return response;
        }
    }
}
