using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class ColaboradorProfissionalRepository : IColaboradorProfissionalRepository
    {
        private readonly DbAgenda _dbAgenda;
        public ColaboradorProfissionalRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<PaginacaoDTO<ColaboradorProfissionalDTO>> BuscarPaginadoPorContaId(string contaId, PaginacaoDTO<ColaboradorProfissionalDTO> paginacao)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var sql = @"SELECT DISTINCT
                          colaborador.Nome AS NomeColaborador,
                          cp.Id,
                          cp.ColaboradorId, 
                          CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                          cp.LojaId, 
                          l.RazaoSocial, 
                          cp.ServicoId, 
                          s.Descricao as DescricaoServico, 
                          cp.Descricao,
                          cp.PeriodoInativoInicio,
                          cp.PeriodoInativoFim,
                          cp.DataCadastro,
                          cc.ContaId
                        FROM  vw_colaborador AS colaborador
                        INNER JOIN Colaborador AS c ON c.UsuarioId  = colaborador.UsuarioId
                        INNER JOIN ColaboradorProfissional cp ON cp.ColaboradorId = c.Id 
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id 
                        INNER JOIN Loja l on l.Id = cp.LojaId 
                        LEFT JOIN Servicos s ON s.Id  = cp.ServicoId 
                        WHERE 
                               cc.ContaId = @ContaId
                        AND    
                        	   cp.Status = 1
                        AND
                               colaborador.Status = 1
                        AND (colaborador.Nome LIKE CONCAT('%', @NomeColaborador, '%') OR @NomeColaborador IS NULL OR @NomeColaborador = '')
                        AND (l.RazaoSocial LIKE CONCAT('%', @RazaoSocial, '%') OR @RazaoSocial IS NULL OR @RazaoSocial = '')
                        AND (s.Descricao LIKE CONCAT('%', @DescricaoServico, '%') OR @DescricaoServico IS NULL OR @DescricaoServico = '')
                        AND (cp.Descricao LIKE CONCAT('%', @Descricao, '%') OR @Descricao IS NULL OR @Descricao = '')
                        ORDER BY colaborador.Nome ";

            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                ContaId = contaId,
                NomeColaborador = paginacao?.objetoPesquisa?.NomeColaborador,
                RazaoSocial = paginacao?.objetoPesquisa?.RazaoSocial,
                DescricaoServico = paginacao?.objetoPesquisa?.DescricaoServico,
                Descricao = paginacao?.objetoPesquisa?.Descricao
            };

            var retorno = await _dbAgenda.Connection.QueryAsync<ColaboradorProfissionalDTO>(sql, parametros) ?? Enumerable.Empty<ColaboradorProfissionalDTO>();
            paginacao.total = retorno.Count();
            paginacao.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;

            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao.objetos = await _dbAgenda.Connection.QueryAsync<ColaboradorProfissionalDTO>(sqlBusca, parametros) ?? Enumerable.Empty<ColaboradorProfissionalDTO>();

            return paginacao;
        }

        public async Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorLojaId(int id)
        {
            var sql = @"SELECT DISTINCT 
                                cp.Id,
                                cp.ColaboradorId, 
                                CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                                cp.LojaId, 
                                cp.ServicoId, 
                                s.Descricao as DescricaoServico,
                                cp.PeriodoInativoInicio,
                                cp.PeriodoInativoFim
                       FROM ColaboradorProfissional cp
                       INNER JOIN Colaborador c ON c.Id  = cp.ColaboradorId 
                       INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id 
                       INNER JOIN Loja l on l.Id = cp.LojaId 
                       LEFT JOIN Servicos s ON s.Id  = cp.ServicoId 
                       WHERE 
                              l.Id = @Id
                       AND    cp.Status = 1";
            var retorno = await _dbAgenda.Connection.QueryAsync<ColaboradorProfissionalDTO>(
                sql,
                new { Id = id }
            ) ?? Enumerable.Empty<ColaboradorProfissionalDTO>();

            return retorno;
        }
        public async Task<ColaboradorProfissionalDTO> BuscarPorId(int id)
        {
            var sql = @"SELECT cp.Id, cp.ColaboradorId, cp.LojaId, l.RazaoSocial, cp.ServicoId, s.Descricao as DescricaoServico,
                               cp.Descricao, cp.PeriodoInativoInicio, cp.PeriodoInativoFim
                        FROM ColaboradorProfissional cp
                            INNER JOIN Colaborador c ON c.Id  = cp.ColaboradorId 
                            INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id 
                            INNER JOIN Loja l on l.Id = cp.LojaId 
                            LEFT JOIN Servicos s ON s.Id  = cp.ServicoId 
                            WHERE cp.Status = 1
                            AND cp.Id = @Id";
            var retorno = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<ColaboradorProfissionalDTO>(sql, new { Id = id }) ?? new ColaboradorProfissionalDTO();
            return retorno;
        }
        public async Task<int> Salvar(ColaboradorProfissional colaborador)
        {
            var sql = @"INSERT INTO ColaboradorProfissional (ColaboradorId, LojaId, ServicoId, Descricao, Status, DataCadastro, DataAtualizacao, PeriodoInativoInicio, PeriodoInativoFim) VALUES
                                   (@ColaboradorId, @LojaId, @ServicoId, @Descricao, @Status, @DataCadastro, @DataAtualizacao, @PeriodoInativoInicio, @PeriodoInativoFim);
                                    SELECT LAST_INSERT_ID();";

            var parametros = new
            {
                ColaboradorId = colaborador.ColaboradorId,
                LojaId = colaborador.LojaId,
                ServicoId = colaborador.ServicoId,
                Descricao = colaborador.Descricao,
                Status = colaborador.Status,
                DataCadastro = colaborador.DataCadastro,
                DataAtualizacao = colaborador.DataAtualizacao,
                PeriodoInativoInicio = colaborador.PeriodoInativoInicio,
                PeriodoInativoFim = colaborador.PeriodoInativoFim
            };
            var retorno = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, parametros, _dbAgenda.Transaction);
            return retorno;
        }


        public async Task<bool> Atualizar(ColaboradorProfissional colaborador)
        {
            var sql = @"UPDATE ColaboradorProfissional
                            SET 
                                ColaboradorId = @ColaboradorId,
                                LojaId = @LojaId,
                                ServicoId = @ServicoId,
                                Descricao = @Descricao,
                                DataAtualizacao = @DataAtualizacao,
                                PeriodoInativoInicio = @PeriodoInativoInicio,
                                PeriodoInativoFim = @PeriodoInativoFim
                            WHERE 
                                Id = @Id;";
            var parametros = new
            {
                ColaboradorId = colaborador.ColaboradorId,
                LojaId = colaborador.LojaId,
                ServicoId = colaborador.ServicoId,
                Descricao = colaborador.Descricao,
                DataCadastro = colaborador.DataCadastro,
                DataAtualizacao = colaborador.DataAtualizacao,
                PeriodoInativoInicio = colaborador.PeriodoInativoInicio,
                PeriodoInativoFim = colaborador.PeriodoInativoFim,
                Id = colaborador.Id,
            };
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, parametros) > 0;
            return retorno;
        }
        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE ColaboradorProfissional SET 
                      Status = 0 
                      Where Id = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            { Id = id }) > 0;
            return retorno;
        }

        public async Task<IEnumerable<ColaboradorServicos>> BuscarServicosPorColaboradorId(int colaboradorId)
        {
            var sql = @"SELECT Id, IdColaborador, IdServico, DataCadastro
                        FROM ColaboradorServicos
                        WHERE IdColaborador = @IdColaborador";

            return await _dbAgenda.Connection.QueryAsync<ColaboradorServicos>(
                sql,
                new { IdColaborador = colaboradorId },
                _dbAgenda.Transaction) ?? Enumerable.Empty<ColaboradorServicos>();
        }

        public async Task<int> SalvarServico(ColaboradorServicos colaboradorServico)
        {
            var sql = @"INSERT INTO ColaboradorServicos (IdColaborador, IdServico, DataCadastro)
                        VALUES (@IdColaborador, @IdServico, @DataCadastro);
                        SELECT LAST_INSERT_ID();";

            return await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                colaboradorServico.IdColaborador,
                colaboradorServico.IdServico,
                colaboradorServico.DataCadastro
            }, _dbAgenda.Transaction);
        }

        public async Task<bool> RemoverServico(int colaboradorId, int servicoId)
        {
            var sql = @"DELETE FROM ColaboradorServicos
                        WHERE IdColaborador = @IdColaborador
                          AND IdServico = @IdServico;";

            return await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                IdColaborador = colaboradorId,
                IdServico = servicoId
            }, _dbAgenda.Transaction) > 0;
        }

        public async Task<bool> RemoverTodosServicos(int colaboradorId)
        {
            var sql = @"DELETE FROM ColaboradorServicos
                        WHERE IdColaborador = @IdColaborador;";

            await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                IdColaborador = colaboradorId
            }, _dbAgenda.Transaction);

            return true;
        }

        public async Task<PaginacaoDTO<ColaboradorProfissionalDTO>> BuscarPaginadoPorUsuario(string usuarioId, PaginacaoDTO<ColaboradorProfissionalDTO> paginacao)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var sql = @"SELECT DISTINCT
                          colaborador.Nome AS NomeColaborador,
                          cp.Id,
                          cp.ColaboradorId, 
                          CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                          cp.LojaId, 
                          l.RazaoSocial, 
                          cp.ServicoId, 
                          s.Descricao as DescricaoServico, 
                          cp.Descricao,
                          cp.PeriodoInativoInicio,
                          cp.PeriodoInativoFim,
                          cp.DataCadastro,
                          cc.ContaId
                        FROM vw_colaborador AS colaborador
                        INNER JOIN Colaborador AS c ON c.UsuarioId  = colaborador.UsuarioId
                        INNER JOIN ColaboradorProfissional cp ON cp.ColaboradorId = c.Id 
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id 
                        INNER JOIN Loja l on l.Id = cp.LojaId 
                        LEFT JOIN Servicos s ON s.Id  = cp.ServicoId 
                        WHERE 
                               colaborador.UsuarioId  = @UsuarioId
                        AND    
                        	   cp.Status = 1
                        AND
                               colaborador.Status = 1
                        AND (colaborador.Nome LIKE CONCAT('%', @NomeColaborador, '%') OR @NomeColaborador IS NULL OR @NomeColaborador = '')
                        AND (l.RazaoSocial LIKE CONCAT('%', @RazaoSocial, '%') OR @RazaoSocial IS NULL OR @RazaoSocial = '')
                        AND (s.Descricao LIKE CONCAT('%', @DescricaoServico, '%') OR @DescricaoServico IS NULL OR @DescricaoServico = '')
                        AND (cp.Descricao LIKE CONCAT('%', @Descricao, '%') OR @Descricao IS NULL OR @Descricao = '')
                        ORDER BY colaborador.Nome ";

            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                UsuarioId = usuarioId,
                NomeColaborador = paginacao?.objetoPesquisa?.NomeColaborador,
                RazaoSocial = paginacao?.objetoPesquisa?.RazaoSocial,
                DescricaoServico = paginacao?.objetoPesquisa?.DescricaoServico,
                Descricao = paginacao?.objetoPesquisa?.Descricao
            };

            var retorno = await _dbAgenda.Connection.QueryAsync<ColaboradorProfissionalDTO>(sql, parametros) ?? Enumerable.Empty<ColaboradorProfissionalDTO>();
            paginacao.total = retorno.Count();
            paginacao.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;

            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao.objetos = await _dbAgenda.Connection.QueryAsync<ColaboradorProfissionalDTO>(sqlBusca, parametros) ?? Enumerable.Empty<ColaboradorProfissionalDTO>();

            return paginacao;
        }

        public async Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorContaId(string contaId)
        {
            var sql = @"SELECT DISTINCT
                          colaborador.Nome AS NomeColaborador,
                          cp.ColaboradorId AS Id, 
                          CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                          cp.LojaId, 
                          l.RazaoSocial, 
                          cp.ServicoId, 
                          s.Descricao as DescricaoServico, 
                          cp.Descricao,
                          cp.PeriodoInativoInicio,
                          cp.PeriodoInativoFim,
                          cp.DataCadastro,
                          cc.ContaId
                        FROM  vw_colaborador AS colaborador
                        INNER JOIN Colaborador AS c ON c.UsuarioId  = colaborador.UsuarioId
                        INNER JOIN ColaboradorProfissional cp ON cp.ColaboradorId = c.Id 
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id 
                        INNER JOIN Loja l on l.Id = cp.LojaId 
                        LEFT JOIN Servicos s ON s.Id  = cp.ServicoId 
                        WHERE 
                               cc.ContaId = @ContaId
                        AND    
                        	   cp.Status = 1
                        AND
                               colaborador.Status = 1";
            var retorno = await _dbAgenda.Connection.QueryAsync<ColaboradorProfissionalDTO>(
                sql,
                new { ContaId = contaId }
            ) ?? Enumerable.Empty<ColaboradorProfissionalDTO>();

            return retorno;
        }

        public async Task<IEnumerable<ColaboradorProfissionalDTO>> BuscarPorUsuarioId(string usuarioId)
        {
            var sql = @"SELECT DISTINCT
                          colaborador.Nome AS NomeColaborador,
                          cp.Id,
                          cp.ColaboradorId, 
                          CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                          cp.LojaId, 
                          l.RazaoSocial, 
                          cp.ServicoId, 
                          s.Descricao as DescricaoServico, 
                          cp.Descricao,
                          cp.PeriodoInativoInicio,
                          cp.PeriodoInativoFim,
                          cp.DataCadastro,
                          cc.ContaId
                        FROM vw_colaborador AS colaborador
                        INNER JOIN Colaborador AS c ON c.UsuarioId  = colaborador.UsuarioId
                        INNER JOIN ColaboradorProfissional cp ON cp.ColaboradorId = c.Id 
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id 
                        INNER JOIN Loja l on l.Id = cp.LojaId 
                        LEFT JOIN Servicos s ON s.Id  = cp.ServicoId 
                        WHERE 
                               colaborador.UsuarioId  = @UsuarioId
                        AND    
                        	   cp.Status = 1
                        AND
                               colaborador.Status = 1";
            var retorno = await _dbAgenda.Connection.QueryAsync<ColaboradorProfissionalDTO>(
                sql,
                new { UsuarioId = usuarioId }
            ) ?? Enumerable.Empty<ColaboradorProfissionalDTO>();

            return retorno;
        }
    }
}
