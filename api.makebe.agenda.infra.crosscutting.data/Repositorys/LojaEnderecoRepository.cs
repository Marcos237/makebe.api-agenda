using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using Dapper;
using StackExchange.Redis;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class LojaEnderecoRepository : IEnderecoContextRepository<LojaEndereco, EnderecoDTO>
    {
        private readonly DbAgenda _dbAgenda;
        public LojaEnderecoRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<IEnumerable<EnderecoDTO>> BuscarEnderecos(string contaId)
        {
            var sql = @"SELECT e.*, le.LojaId, l.RazaoSocial, CAST(cl.ContaId AS CHAR) AS ContaId, le.Id AS LojaEnderecoId FROM Endereco e
                          INNER JOIN LojaEndereco le ON e.Id = le.EnderecoId
                          INNER JOIN ContaLoja cl ON cl.LojaId = le.LojaId 
                          INNER JOIN Loja l ON l.Id = le.LojaId 
                        WHERE cl.ContaId = @ContaId
                        AND e.Status = 1;";
            var enderecos = await _dbAgenda.Connection.QueryAsync<EnderecoDTO>(sql, new { ContaId = contaId }) ?? Enumerable.Empty<EnderecoDTO>();
            return enderecos;
        }
        public async Task<int> Salvar(LojaEndereco endereco)
        {
            var query = @"INSERT INTO LojaEndereco (LojaId, EnderecoId, DataCadastro) VALUES (@LojaId, @EnderecoId, @DataCadastro);
                           SELECT LAST_INSERT_ID();";
            var parameters = new
            {
                LojaId = endereco.LojaId,
                EnderecoId = endereco.EnderecoId,
                DataCadastro = endereco.DataCadastro

            };
            var result = await _dbAgenda.Connection.ExecuteScalarAsync<int>(query, parameters, _dbAgenda.Transaction);
            return result;

        }
        public async Task<bool> Atualizar(LojaEndereco endereco)
        {
            var query = @"UPDATE LojaEndereco  SET LojaId = @LojaId, DataCadastro = @DataCadastro WHERE Id = @EnderecoId";
            var result = await _dbAgenda.Connection.ExecuteAsync(query, endereco) > 0;
            return result;
        }
    }
}
