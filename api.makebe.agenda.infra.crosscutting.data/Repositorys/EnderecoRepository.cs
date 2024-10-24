using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly DbAgenda _dbAgenda;
        public EnderecoRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao ,string usuarioId)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;
            var sql = await BuscarConsulta();
            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                Logradouro = paginacao?.objetoPesquisa?.Logradouro,
                LojaId = paginacao?.objetoPesquisa?.LojaId,
                UsuarioId = usuarioId
            };
            var enderecos = await _dbAgenda.Connection.QueryAsync<EnderecoDTO>(sql, parametros) ?? Enumerable.Empty<EnderecoDTO>();
            paginacao!.total = enderecos.Count();
            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao!.objetos = await _dbAgenda.Connection.QueryAsync<EnderecoDTO>(sqlBusca, parametros) ?? Enumerable.Empty<EnderecoDTO>();
            return paginacao;
        }

        public async Task<EnderecoDTO> BuscarPorId(int id)
        {
            var query = @"SELECT e.*, le.LojaId FROM  UsuarioLoja ul 
                            INNER JOIN LojaEndereco le ON ul.LojaId  = le.LojaId 
                            INNER JOIN Endereco e ON e.Id  = le.EnderecoId 
                            WHERE e.Id = @Id";
            var result = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<EnderecoDTO>(query, new { Id = id }) ?? new EnderecoDTO();
            return result;
        }

        public async Task<int> Salvar(Endereco endereco)
        {
            var query = @"INSERT INTO Endereco (Logradouro, Numero, Complemento, CEP, Estado, Cidade, Status, DataCadastro, DataAtualizacao) 
                          VALUES(@Logradouro, @Numero, @Complemento, @CEP, @Estado, @Cidade, @Status, @DataCadastro, @DataAtualizacao);
                           SELECT LAST_INSERT_ID() AS LastInsertedId;";
            var result = await _dbAgenda.Connection.ExecuteScalarAsync<int>(query, endereco);

            return result;
        }
        public async Task<Endereco> Atualizar(Endereco endereco)
        {
            var query = @"UPDATE Endereco SET
                            Logradouro = @Logradouro, 
                            Numero = @Numero, 
                            Complemento = @Complemento, 
                            CEP = @CEP,
                            Estado = @Estado,  
                            Cidade = @Cidade,
                            Status = @Status, 
                            DataCadastro = @DataCadastro, 
                            DataAtualizacao = @DataAtualizacao
                            WHERE Id = @Id";
            var result = await _dbAgenda.Connection.ExecuteAsync(query, endereco);
            return endereco;
        }

        public async Task<bool> Deastivar(int id)
        {
            var query = @"UPDATE Endereco  SET
                            Status = 0 
                            WHERE Id = @Id";
            var result = await _dbAgenda.Connection.ExecuteAsync(query, new {Id = id}) > 0;
            return result;
        }

        private Task<string> BuscarConsulta()
        {
            var query = @"SELECT DISTINCT e.*, le.LojaId , l.RazaoSocial 
                            FROM UsuarioLoja ul 
                            INNER JOIN LojaEndereco le ON ul.LojaId = le.LojaId 
                            INNER JOIN Endereco e ON e.Id = le.EnderecoId 
                            Inner join Loja l on l.Id = le.LojaId 
                            WHERE ul.UsuarioId = @UsuarioId
                            AND 
                                (e.Logradouro LIKE CONCAT('%', @Logradouro, '%') OR @Logradouro IS NULL OR @Logradouro = '')
                            AND 
                                (@LojaId IS NULL OR @LojaId = 0 OR le.LojaId = @LojaId)
                            AND e.Status = 1
                            ORDER BY e.Logradouro ";
            return Task.FromResult(query);
        }

    }
}
