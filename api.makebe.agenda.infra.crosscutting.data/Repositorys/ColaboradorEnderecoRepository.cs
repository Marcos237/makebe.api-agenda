using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ColaboradorEnderecoRepository : IEnderecoContextRepository<ColaboradorEndereco, EnderecoDTO>, IColaboradorEnderecoRepository
    {
        private readonly DbAgenda _dbAgenda;

        public ColaboradorEnderecoRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<IEnumerable<EnderecoDTO>> BuscarEnderecos(string contaId)
        {
            var paginacao = await BuscarEnderecos(new PaginacaoDTO<EnderecoDTO>
            {
                paginaAtual = 1,
                quantidadePagina = int.MaxValue,
                objetoPesquisa = new EnderecoDTO()
            }, contaId);

            return paginacao.objetos ?? Enumerable.Empty<EnderecoDTO>();
        }

        public async Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao, string contaId)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var sql = @"SELECT DISTINCT
                            e.Id,
                            e.Logradouro,
                            e.Numero,
                            e.Complemento,
                            e.CEP,
                            e.Estado,
                            e.Cidade,
                            ce.Id AS ColaboradorEnderecoId,
                            ce.ColaboradorId,
                            CAST(cc.ContaId AS CHAR) AS ContaId,
                            CAST(vc.UsuarioId AS CHAR) AS UsuarioId,
                            vc.Nome AS NomeColaborador,
                            2 AS TipoUsuarioId
                        FROM Endereco e
                        INNER JOIN ColaboradorEndereco ce ON ce.EnderecoId = e.Id
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId = ce.ColaboradorId
                        INNER JOIN Colaborador c ON c.Id = ce.ColaboradorId
                        INNER JOIN vw_colaborador vc ON vc.UsuarioId = c.UsuarioId
                        WHERE cc.ContaId = @ContaId
                          AND e.Status = 1
                          AND (vc.Nome LIKE CONCAT('%', @NomeColaborador, '%') OR @NomeColaborador IS NULL OR @NomeColaborador = '')
                          AND (e.Logradouro LIKE CONCAT('%', @Logradouro, '%') OR @Logradouro IS NULL OR @Logradouro = '')
                        ORDER BY vc.Nome, e.Logradouro";

            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                ContaId = contaId,
                NomeColaborador = paginacao?.objetoPesquisa?.NomeColaborador,
                Logradouro = paginacao?.objetoPesquisa?.Logradouro
            };

            var enderecos = await _dbAgenda.Connection.QueryAsync<EnderecoDTO>(sql, parametros) ?? Enumerable.Empty<EnderecoDTO>();
            paginacao.total = enderecos.Count();
            paginacao.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;

            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao.objetos = await _dbAgenda.Connection.QueryAsync<EnderecoDTO>(sqlBusca, parametros) ?? Enumerable.Empty<EnderecoDTO>();

            return paginacao;
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
