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
        public async Task<IEnumerable<Endereco>> BuscarEnderecos(PaginacaoDTO<Endereco> paginacao ,string usuarioId)
        {
            var query = @"SELECT e.* FROM  UsuarioLoja ul 
                            INNER JOIN LojaEndereco le ON ul.LojaId  = le.LojaId 
                            INNER JOIN Endereco e ON e.Id  = le.LojaId 
                            WHERE ul.UsuarioId = @UsuarioId
                            AND 
                            (e.Logradouro LIKE '%' + @Logradoruro + '%' OR @Logradoruro IS NULL OR @Logradoruro = '')
                            ORDER BY e.Logradouro 
                            OFFSET @RegistroInicial ROWS
                            FETCH NEXT @TamanhoPagina ROWS ONLY";
            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                Logradouro = paginacao?.objetoPesquisa?.Logradouro,
                UsuarioId = usuarioId
            };
            paginacao!.objetos = await _dbAgenda.Connection.QueryAsync<Endereco>(query, parametros) ?? Enumerable.Empty<Endereco>();
            return paginacao.objetos;
        }

        public async Task<Endereco> BuscarPorId(int id)
        {
            var query = @"SELECT e.* FROM  UsuarioLoja ul 
                            INNER JOIN LojaEndereco le ON ul.LojaId  = le.LojaId 
                            INNER JOIN Endereco e ON e.Id  = le.LojaId 
                            WHERE e.Id = @Id";
            var result = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<Endereco>(query, new { Id = id }) ?? new Endereco();
            return result;
        }
        public async Task<int> Salvar(Endereco endereco)
        {
            var query = @"INSERT INTO Endereco (Logradouro, Numero, Complemento, CEP, Estado, Cidade, Status, DataCadastro, DataAtualizacao) 
                          VALUES(@Logradouro, @Numero, @Complemento, @CEP, @Estado, @Cidade, @Status, @DataCadastro, @DataAtualizacao)
                           SELECT LAST_INSERT_ID() AS LastInsertedId;";
            var result = await _dbAgenda.Connection.ExecuteAsync(query, endereco);

            return result;
        }
        public async Task<Endereco> Atualizar(Endereco endereco)
        {
            var query = @"UPDATE Endereco SET
                            Logradouro = @Logradouro, 
                            Numero = @Numero, 
                            Complemento = @Complemento, 
                            CEP = @CEP,
                            Estado = @Estado  
                            Cidade = @Cidade,, 
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
                            Status = 0, 
                            WHERE Id = @Id";
            var result = await _dbAgenda.Connection.ExecuteAsync(query, new {Id = id}) > 0;
            return result;
        }
    }
}
