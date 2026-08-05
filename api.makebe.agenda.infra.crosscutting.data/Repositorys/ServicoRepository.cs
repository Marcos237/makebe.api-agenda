using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;
using MassTransit;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ServicoRepository : IServicosRepository
    {
        private readonly DbAgenda _dbAgenda;
        public ServicoRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<PaginacaoDTO<ServicoDTO>> BuscarPaginado(PaginacaoDTO<ServicoDTO> paginacao, string contaId)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;
            var sql = await BuscarConsulta();
            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                Descricao = paginacao?.objetoPesquisa?.Descricao,
                Periodo = paginacao?.objetoPesquisa?.Periodo,
                Valor = paginacao?.objetoPesquisa?.Valor,
                ContaId = contaId
            };

            var servicos = await _dbAgenda.Connection.QueryAsync<ServicoDTO>(sql, parametros) ?? Enumerable.Empty<ServicoDTO>();
            paginacao!.total = servicos.Count();

            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao!.objetos = await _dbAgenda.Connection.QueryAsync<ServicoDTO>(sqlBusca, param: parametros) ?? Enumerable.Empty<ServicoDTO>();

            return paginacao;

        }

        public async Task<Servico> BuscarPorId(int id)
        {
            var sql = @"SELECT s.* from Servicos s  WHERE s.Status = 1 AND s.Id = @Id";
            var response = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<Servico>(sql, new { Id = id }, _dbAgenda.Transaction) ?? new Servico();
            return response;
        }

        public async Task<IEnumerable<Servico>> BuscarServicos(string contaId)
        {
            var sql = @"SELECT s.ID, s.Descricao, s.Valor, s.Periodo FROM  Servicos s
                        INNER JOIN ContaServico cs  ON cs.ServicoId  = s.Id 
                        WHERE ContaId = @ContaId  AND s.Status = 1";
            var retorno = await _dbAgenda.Connection.QueryAsync<Servico>(sql, new { ContaId = contaId }) ?? Enumerable.Empty<Servico>();
            return retorno;
        }
        public async Task<IEnumerable<Servico>> BuscarServicosPorColaboradorId(int id)
        {
            var sql = @"SELECT s.Id, s.Descricao, s.Valor, s.Periodo
                        FROM Servicos s
                        INNER JOIN ColaboradorServicos cs ON cs.IdServico = s.Id
                        WHERE cs.IdColaborador = @IdColaborador
                          AND s.Status = 1";
            var retorno = await _dbAgenda.Connection.QueryAsync<Servico>(sql, new { IdColaborador = id }) ?? Enumerable.Empty<Servico>();
            return retorno;
        }

        public async Task<int> Salvar(Servico servicos)
        {
            var sql = @"INSERT INTO Servicos (Descricao, Status, DataCadastro, DataAtualizacao, Periodo, Valor)
                            VALUES (@Descricao, @Status, @DataCadastro, @DataAtualizacao, @Periodo, @Valor);
                            SELECT LAST_INSERT_ID() AS LastInsertedId;";
            var response = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                Descricao = servicos.Descricao,
                Status = servicos.Status,
                DataCadastro = servicos.DataCadastro,
                DataAtualizacao = servicos.DataAtualizacao,
                Periodo = servicos.Periodo,
                Valor = servicos.Valor,
            }, _dbAgenda.Transaction);

            return response;
        }
        public async Task<Servico> Atualizar(Servico servicos)
        {
            var sql = @"UPDATE Servicos 
                            SET 
                            Descricao = @Descricao, 
                            DataAtualizacao = @DataAtualizacao,
                            Periodo = @Periodo,
                            Valor = @Valor
                            WHERE Id = @Id";
            var response = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                Descricao = servicos.Descricao,
                DataAtualizacao = servicos.DataAtualizacao,
                Periodo = servicos.Periodo,
                Valor = servicos.Valor,
                Id = servicos.Id
            }, _dbAgenda.Transaction);
            return servicos;
        }

        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE Servicos SET Status = 0 WHERE Id = @Id";
            var response = await _dbAgenda.Connection.ExecuteAsync(sql, new { Id = id }, _dbAgenda.Transaction);
            return response > 0;
        }

        private async Task<string> BuscarConsulta()
        {
            var query = @"SELECT 
                            s.Id, 
                            s.Descricao,
                            DATE_FORMAT(s.DataCadastro, '%d/%m/%Y %H:%i:%s') AS DataCadastro,
                            s.Periodo,
                            s.Valor,
                            ci.Id AS CategoriaItemId,
                            ci.Descricao AS DescricaoCategoria
                        FROM Servicos s
                        INNER JOIN ContaServico cs ON cs.ServicoId = s.Id
                        LEFT JOIN Categoria c ON c.IdServico = s.Id AND c.Ativo = 1
                        LEFT JOIN CategoriaItem ci ON ci.id = c.CategoriaItemId AND ci.Status = 1
                        WHERE s.Status = 1
                          AND cs.ContaId = @ContaId
                          AND (
                                (@Descricao IS NULL OR @Descricao = '' OR s.Descricao LIKE CONCAT('%', @Descricao, '%'))
                              )
                          AND (
                                @Valor IS NULL OR @Valor = 0 OR s.Valor = @Valor
                              )
                          AND (
                                @Periodo IS NULL OR @Periodo = 0 OR s.Periodo = @Periodo
                              )
                        ORDER BY s.Descricao";
            return await Task.FromResult(query);
        }
    }
}
