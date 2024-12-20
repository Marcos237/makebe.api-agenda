using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class LojaPortifolioRepository : ILojaPortifolioRepository
    {
        private readonly DbAgenda _dbAgenda;
        public LojaPortifolioRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<PaginacaoDTO<LojaPortifolioDTO>> BuscarLojaPortifolios(PaginacaoDTO<LojaPortifolioDTO> paginacao, string contaId)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;
            var imagens = new List<LojaPortifolioImagemDTO>();
            var sql = await BuscarConsulta();
            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                Titulo = paginacao?.objetoPesquisa?.Titulo,
                SubTitulo = paginacao?.objetoPesquisa?.SubTitulo,
                LojaId = paginacao?.objetoPesquisa?.LojaId,
                ContaId = contaId
            };
            var lojaPortifolio = await _dbAgenda.Connection.QueryAsync<LojaPortifolioDTO>(sql, parametros) ?? Enumerable.Empty<LojaPortifolioDTO>();
            paginacao!.total = lojaPortifolio.Count();
            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            var retorno = await _dbAgenda.Connection.QueryAsync<LojaPortifolioDTO>(sqlBusca, parametros) ?? Enumerable.Empty<LojaPortifolioDTO>();

            paginacao.objetos = retorno;
            return paginacao;
        }

        public async Task<LojaPortifolioDTO> BuscarPorId(int id)
        {
            var query = @"SELECT * FROM LojaPortifolio lp  where Id = @Id";
            var result = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<LojaPortifolioDTO>(query, new { Id = id }) ?? new LojaPortifolioDTO();
            return result;
        }

        public async Task<int> Salvar(LojaPortifolio portifolio)
        {
            var query = @"INSERT INTO LojaPortifolio (LojaId, Titulo, SubTitulo, Texto, Status, DataCadastro, DataAtualizacao)
                          VALUES 
                          (@LojaId, @Titulo, @SubTitulo, @Texto, @Status, @DataCadastro, @DataAtualizacao);
                        SELECT LAST_INSERT_ID();";
            var result = await _dbAgenda.Connection.ExecuteScalarAsync<int>(query, portifolio, _dbAgenda.Transaction);

            return result;
        }
        public async Task<LojaPortifolio> Atualizar(LojaPortifolio portifolio)
        {
            var query = @"UPDATE LojaPortifolio  SET 
                          LojaId  = @LojaId,
                          Titulo  = @Titulo,
                          SubTitulo  = @SubTitulo,
                          Texto  = @Texto,
                          DataCadastro  = @DataCadastro
                          WHERE Id = @Id";
            var result = await _dbAgenda.Connection.ExecuteAsync(query, portifolio);
            return portifolio;
        }

        public async Task<bool> Deastivar(int id)
        {
            var query = @"UPDATE LojaPortifolio  SET 
                          Status  = 0
                          WHERE Id = @Id
                          ";
            var result = await _dbAgenda.Connection.ExecuteAsync(query, new { Id = id }) > 0;
            return result;
        }

        private Task<string> BuscarConsulta()
        {
            var query = @"
                        SELECT DISTINCT lp.Id, lp.LojaId, lp.Titulo, lp.SubTitulo, l.RazaoSocial FROM LojaPortifolio lp 
                              INNER JOIN ContaLoja ul  ON ul.LojaId = lp.LojaId 
                              INNER JOIN  Loja l  ON l.Id  = lp.LojaId 
                              WHERE ul.ContaId  = @ContaId
                                 AND 
                                                        (lp.Titulo LIKE CONCAT('%', @Titulo, '%') OR @Titulo IS NULL OR @Titulo = '')
                                 AND 
                                                        (lp.SubTitulo LIKE CONCAT('%', @SubTitulo, '%') OR @SubTitulo IS NULL OR @SubTitulo = '')
                                 AND 
                                                        (@LojaId IS NULL OR @LojaId = 0 OR lp.LojaId = @LojaId)
                                 AND lp.Status = 1
                             ORDER BY lp.Titulo ";
            return Task.FromResult(query);
        }
    }
}
