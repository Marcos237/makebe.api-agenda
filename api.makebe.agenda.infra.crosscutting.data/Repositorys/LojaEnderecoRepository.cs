using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class LojaEnderecoRepository : IEnderecoContextRepository<LojaEndereco, EnderecoDTO>, IEnderecoLojaRepository
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

        public async Task<IEnumerable<EnderecoLojaDTO>> BuscarEnderecoLoja(int id)
        {
            var sql = @"SELECT DISTINCT 
                                    l.Id,
                                    l.RazaoSocial,
                                    l.Telefone,
                                    l.Email,
                                    p.Texto,
                                    e.CEP,
                                    e.Cidade,
                                    e.Logradouro,
                                    e.Numero,
                                    e.Estado,
                                    e.Complemento,
                                    e.Status
                                FROM Loja l
                                INNER JOIN LojaPortifolio lp 
                                    ON lp.LojaId = l.Id        
                                INNER JOIN Portifolio p ON p.Id  = lp.PortifolioId     
                                INNER JOIN PortifolioImagens pi ON pi.PortifolioId = p.Id
                                LEFT  JOIN LojaEndereco le on le.LojaId = l.Id
                                LEFT  JOIN Endereco e on e.Id = le.EnderecoId
                                WHERE l.Id = @Id
                                  AND l.Status = 1
                                  AND lp.Status = 1
                                  AND pi.Status = 1
                                  AND e.Status = 1";

            var retorno = await _dbAgenda.Connection.QueryAsync<EnderecoLojaDTO>(sql, new { Id = id })
                ?? Enumerable.Empty<EnderecoLojaDTO>();

            return retorno;
        }
    }
}
