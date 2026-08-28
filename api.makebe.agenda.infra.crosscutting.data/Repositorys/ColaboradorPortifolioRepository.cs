using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ColaboradorPortifolioRepository : IPortifolioContextRepository<ColaboradorPortifolio, PortifolioDTO>, IColaboradorPortifolioRepository
    {
        private readonly DbAgenda _dbAgenda;

        public ColaboradorPortifolioRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<IEnumerable<PortifolioDTO>> BuscarPortifolios(string contaId)
        {
            var paginacao = await BuscarPortifolios(new PaginacaoDTO<PortifolioDTO>
            {
                paginaAtual = 1,
                quantidadePagina = int.MaxValue,
                objetoPesquisa = new PortifolioDTO()
            }, contaId);

            return paginacao.objetos ?? Enumerable.Empty<PortifolioDTO>();
        }

        public async Task<PaginacaoDTO<PortifolioDTO>> BuscarPortifolios(PaginacaoDTO<PortifolioDTO> paginacao, string contaId)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var sql = @"SELECT DISTINCT p.Id,
                             CAST(cc.ContaId AS CHAR) AS ContaId,
                             p.Titulo,
                             p.SubTitulo,
                             CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                             cp.Id AS ColaboradorPortifolioId,
                             p.Id AS PortifolioId,
                             c.Id AS ColaboradorId,
                             vc.Nome AS NomeColaborador,
                             2 AS TipoUsuarioId,
                             c.Ativo,
                             p.Texto
                        FROM `Makebe.Agenda`.Portifolio p
                        INNER JOIN `Makebe.Agenda`.ColaboradorPortifolio cp ON cp.PortifolioId = p.Id
                        INNER JOIN `Makebe.Agenda`.Colaborador c ON c.Id = cp.ColaboradorId
                        INNER JOIN `Makebe.Agenda`.ContaColaborador cc ON cc.ColaboradorId = c.Id
                        INNER JOIN `Makebe.Agenda`.vw_colaborador vc ON vc.UsuarioId = c.UsuarioId
                        WHERE cc.ContaId = @ContaId
                          AND p.Status = 1
                          AND (vc.Nome LIKE CONCAT('%', @NomeColaborador, '%') OR @NomeColaborador IS NULL OR @NomeColaborador = '')
                          AND (p.Titulo LIKE CONCAT('%', @Titulo, '%') OR @Titulo IS NULL OR @Titulo = '')
                          AND (p.SubTitulo LIKE CONCAT('%', @SubTitulo, '%') OR @SubTitulo IS NULL OR @SubTitulo = '')
                        ORDER BY vc.Nome, p.Titulo";

            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                ContaId = contaId,
                NomeColaborador = paginacao?.objetoPesquisa?.NomeColaborador,
                Titulo = paginacao?.objetoPesquisa?.Titulo,
                SubTitulo = paginacao?.objetoPesquisa?.SubTitulo
            };

            var portifolios = await _dbAgenda.Connection.QueryAsync<PortifolioDTO>(sql, parametros) ?? Enumerable.Empty<PortifolioDTO>();
            paginacao.total = portifolios.Count();
            paginacao.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;

            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao.objetos = await _dbAgenda.Connection.QueryAsync<PortifolioDTO>(sqlBusca, parametros) ?? Enumerable.Empty<PortifolioDTO>();

            return paginacao;
        }

        public async Task<int> Salvar(ColaboradorPortifolio item)
        {
            var sql = @"INSERT INTO ColaboradorPortifolio (ColaboradorId, PortifolioId, Status, DataCadastro) VALUES (@ColaboradorId, @PortifolioId, @Status, @DataCadastro);
                        SELECT LAST_INSERT_ID();";
            var parametros = new
            {
                ColaboradorId = item.ColaboradorId,
                PortifolioId = item.PortifolioId,
                Status = item.Status,
                DataCadastro = item.DataCadastro
            };
            var response = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, parametros, _dbAgenda.Transaction);
            return response;
        }

        public async Task<bool> Atualizar(ColaboradorPortifolio item)
        {
            var sql = @"UPDATE ColaboradorPortifolio  SET 
                                  ColaboradorId = @ColaboradorId,
                                  PortifolioId = @PortifolioId, 
                                  Status = @Status, 
                                  DataCadastro = @DataCadastro
                                  WHERE Id = @ID";
            var parametros = new
            {
                ColaboradorId = item.ColaboradorId,
                PortifolioId = item.PortifolioId,
                Status = item.Status,
                DataCadastro = item.DataCadastro,
                Id = item.Id
            };
            var response = await _dbAgenda.Connection.ExecuteAsync(sql, parametros, _dbAgenda.Transaction) > 0;
            return response;
        }
    }
}
