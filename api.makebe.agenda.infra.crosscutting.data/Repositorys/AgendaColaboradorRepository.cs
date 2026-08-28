using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class AgendaColaboradorRepository : IAgendaContextRepository<AgendaColaborador>, IAgendaColaboradorRepository
    {
        private readonly DbAgenda _dbAgenda;

        public AgendaColaboradorRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<PaginacaoDTO<AgendaDTO>> BuscarPaginado(PaginacaoDTO<AgendaDTO> paginacao, string contaId)
        {
            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;

            var sql = @"SELECT 
                                a.Id,
                                ac.Id AS IdAgendaColaborador,
                                c.Id AS IdColaborador,
                                CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                                CAST(cc.ContaId AS CHAR) AS ContaId,
                                vc.Nome AS Nome,
                                ac.Bloqueado,
                                a.IsTodoDia,
                                DATE_FORMAT(a.AgendaAbertaInicio, '%d/%m/%Y %H:%i:%s') AS AgendaAbertaInicio,
                                DATE_FORMAT(a.AgendaAbertaFim, '%d/%m/%Y %H:%i:%s') AS AgendaAbertaFim,
                                a.IsBloqueadoHoje,
                                DATE_FORMAT(a.AgendaBloqueadaInicio, '%d/%m/%Y %H:%i:%s') AS AgendaBloqueadaInicio,
                                DATE_FORMAT(a.AgendaBloqueadaFim, '%d/%m/%Y %H:%i:%s') AS AgendaBloqueadaFim,
                                a.Status,
                                a.IdAgendaSemanaInicio,
                                a.IdAgendaSemanaFim,
                                as2.Descricao AS DiaInicioSemana,
                                as3.Descricao AS DiaSemanaFim
                        FROM Colaborador c
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId = c.Id
                        INNER JOIN AgendaColaborador ac ON ac.IdColaborador = c.Id
                        INNER JOIN Agenda a ON a.Id = ac.IdAgenda
                        INNER JOIN AgendaSemana as2 ON as2.Id = a.IdAgendaSemanaInicio
                        INNER JOIN AgendaSemana as3 ON as3.Id = a.IdAgendaSemanaFim
                        INNER JOIN vw_colaborador vc ON vc.UsuarioId = c.UsuarioId
                        WHERE cc.ContaId = @ContaId
                          AND a.Status = 1
                          AND (vc.Nome LIKE CONCAT('%', @Nome, '%') OR @Nome IS NULL OR @Nome = '')
                          AND (
                                ((@IdAgendaSemanaInicio IS NULL OR @IdAgendaSemanaInicio = 0) AND (@IdAgendaSemanaFim IS NULL OR @IdAgendaSemanaFim = 0))
                                OR (
                                    @IdAgendaSemanaInicio <= @IdAgendaSemanaFim
                                    AND a.IdAgendaSemanaInicio >= @IdAgendaSemanaInicio
                                    AND a.IdAgendaSemanaInicio <= @IdAgendaSemanaFim
                                )
                                OR (
                                    @IdAgendaSemanaInicio > @IdAgendaSemanaFim
                                    AND (
                                        a.IdAgendaSemanaInicio >= @IdAgendaSemanaInicio
                                        OR a.IdAgendaSemanaInicio <= @IdAgendaSemanaFim
                                    )
                                )
                              )
                          AND (
                                @AgendaAbertaInicio IS NULL
                                OR DATE_FORMAT(a.AgendaAbertaInicio, '%d/%m/%Y %H:%i:%s') = @AgendaAbertaInicio
                                OR (
                                    @AgendaAbertaFim IS NOT NULL
                                    AND DATE_FORMAT(a.AgendaAbertaInicio, '%d/%m/%Y %H:%i:%s') >= @AgendaAbertaInicio
                                    AND DATE_FORMAT(a.AgendaAbertaFim, '%d/%m/%Y %H:%i:%s') <= @AgendaAbertaFim
                                )
                              )
                          AND (
                                @AgendaAbertaFim IS NULL
                                OR DATE_FORMAT(a.AgendaAbertaFim, '%d/%m/%Y %H:%i:%s') = @AgendaAbertaFim
                                OR @AgendaAbertaInicio IS NOT NULL
                              )
                          AND (
                                @AgendaBloqueadaInicio IS NULL
                                OR DATE_FORMAT(a.AgendaBloqueadaInicio, '%d/%m/%Y %H:%i:%s') = @AgendaBloqueadaInicio
                                OR (
                                    @AgendaBloqueadaFim IS NOT NULL
                                    AND DATE_FORMAT(a.AgendaBloqueadaInicio, '%d/%m/%Y %H:%i:%s') >= @AgendaBloqueadaInicio
                                    AND DATE_FORMAT(a.AgendaBloqueadaFim, '%d/%m/%Y %H:%i:%s') >= @AgendaBloqueadaFim
                                )
                              )
                          AND (
                                @AgendaBloqueadaFim IS NULL
                                OR DATE_FORMAT(a.AgendaBloqueadaFim, '%d/%m/%Y %H:%i:%s') = @AgendaBloqueadaFim
                                OR @AgendaBloqueadaInicio IS NOT NULL
                              )
                        ORDER BY vc.Nome, a.Id DESC";

            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                ContaId = contaId,
                Nome = paginacao?.objetoPesquisa?.Nome,
                IdAgendaSemanaInicio = paginacao?.objetoPesquisa?.IdAgendaSemanaInicio,
                IdAgendaSemanaFim = paginacao?.objetoPesquisa?.IdAgendaSemanaFim,
                AgendaAbertaInicio = ValoresHelper.SetDateTimeCustomer(paginacao?.objetoPesquisa?.AgendaAbertaInicio)?.ToString("dd/MM/yyyy HH:mm:ss"),
                AgendaAbertaFim = ValoresHelper.SetDateTimeCustomer(paginacao?.objetoPesquisa?.AgendaAbertaFim)?.ToString("dd/MM/yyyy HH:mm:ss"),
                AgendaBloqueadaInicio = ValoresHelper.SetDateHourMinuteCustomer(paginacao?.objetoPesquisa?.AgendaBloqueadaInicio)?.ToString("dd/MM/yyyy HH:mm:ss"),
                AgendaBloqueadaFim = ValoresHelper.SetDateHourMinuteCustomer(paginacao?.objetoPesquisa?.AgendaBloqueadaFim)?.ToString("dd/MM/yyyy HH:mm:ss")
            };

            var agendas = await _dbAgenda.Connection.QueryAsync<AgendaDTO>(sql, parametros) ?? Enumerable.Empty<AgendaDTO>();
            paginacao.total = agendas.Count();
            paginacao.totalPaginas = (paginacao.total + paginacao.quantidadePagina - 1) / paginacao.quantidadePagina;

            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao.objetos = await _dbAgenda.Connection.QueryAsync<AgendaDTO>(sqlBusca, parametros) ?? Enumerable.Empty<AgendaDTO>();
            return paginacao;
        }

        public async Task<AgendaDTO> BuscarPorId(int id)
        {
            var sql = @"SELECT 
                         a.id,
                         c.Id As IdColaborador, 
                         CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                         ac.Bloqueado, 
                         a.IsTodoDia,
                         a.IdAgendaSemanaInicio ,
                         a.IdAgendaSemanaFim,
                         DATE_FORMAT(a.AgendaAbertaInicio, '%d/%m/%Y %H:%i:%s') AS AgendaAbertaInicio, 
                         DATE_FORMAT(a.AgendaAbertaFim, '%d/%m/%Y %H:%i:%s') AS AgendaAbertaFim,
                         DATE_FORMAT(a.AgendaBloqueadaInicio, '%d/%m/%Y %H:%i:%s') AS AgendaBloqueadaInicio,
                         DATE_FORMAT(a.AgendaBloqueadaFim, '%d/%m/%Y %H:%i:%s') AS AgendaBloqueadaFim,
                         a.IsBloqueadoHoje,
                         a.Status
                        FROM Colaborador c  
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id
                        INNER JOIN AgendaColaborador ac ON ac.IdColaborador  = c.Id
                        INNER JOIN Agenda a  ON a.Id  = ac.IdAgenda
                        WHERE  a.Status  = 1
                        AND a.Id = @Id";
            var response = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<AgendaDTO>(sql, new { Id = id }) ?? new AgendaDTO();
            return response;
        }

        public async Task<int> Salvar(AgendaColaborador item)
        {
            var sql = @"INSERT INTO AgendaColaborador (IdAgenda, IdColaborador, Bloqueado, DataCadastro, DataAtualizacao , Status)
                         VALUES (@IdAgenda, @IdColaborador, @Bloqueado, @DataCadastro, @DataAtualizacao , @Status);
                        SELECT LAST_INSERT_ID();";
            var response = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                IdAgenda = item.IdAgenda,
                IdColaborador = item.IdColaborador,
                Bloqueado = item.Bloqueado,
                DataCadastro = item.DataCadastro,
                DataAtualizacao = item.DataAtualizacao,
                Status = item.Status
            }, _dbAgenda.Transaction);
            return response;
        }

        public async Task<bool> Atualizar(AgendaColaborador item)
        {
            var sql = @"
                        UPDATE AgendaColaborador
                        SET 
                            Bloqueado = @Bloqueado,
                            DataAtualizacao = @DataAtualizacao
                        WHERE Id = @Id";

            var response = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                Bloqueado = item.Bloqueado,
                DataAtualizacao = item.DataAtualizacao,
                Id = item.Id
            }) > 0;
            return response;
        }

        public async Task<AgendaDTO> BuscarPorIdColaborador(int idColaborador)
        {
            var sql = @"SELECT MAX(Id) AS Id FROM AgendaColaborador WHERE IdColaborador = @Id";
            var response = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<AgendaDTO>(sql, new { Id = idColaborador }) ?? new AgendaDTO();
            return response;
        }

        public async Task<AgendaDTO> BuscarAgendaPorColaboradorId(int idColaborador)
        {
            var sql = @"SELECT 
                            a.IsBloqueadoHoje,
                            DATE_FORMAT(a.AgendaAbertaInicio, '%d/%m/%Y %H:%i:%s') AS AgendaAbertaInicio,
                            DATE_FORMAT(a.AgendaAbertaFim, '%d/%m/%Y %H:%i:%s') AS AgendaAbertaFim,
                            DATE_FORMAT(a.AgendaBloqueadaInicio, '%d/%m/%Y %H:%i:%s') AS AgendaBloqueadaInicio,
                            DATE_FORMAT(a.AgendaBloqueadaFim, '%d/%m/%Y %H:%i:%s') AS AgendaBloqueadaFim
                        FROM AgendaColaborador ac
                        INNER JOIN Agenda a ON a.Id = ac.IdAgenda
                        WHERE ac.IdColaborador = @Id
                          AND a.Status = 1
                        ORDER BY a.Id DESC
                        LIMIT 1";

            var response = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<AgendaDTO>(sql, new { Id = idColaborador }) ?? new AgendaDTO();
            return response;
        }

        public async Task<IEnumerable<AgendaDTO>> BuscarAgendamentosPorColaboradorId(int idColaborador)
        {
            var sql = @"SELECT DISTINCT
                            a.Id,
                            ac.Id AS IdAgendaColaborador,
                            a.IsBloqueadoHoje,
                            DATE_FORMAT(a.AgendaAbertaInicio, '%d/%m/%Y %H:%i:%s') AS AgendaAbertaInicio,
                            DATE_FORMAT(a.AgendaAbertaFim, '%d/%m/%Y %H:%i:%s') AS AgendaAbertaFim,
                            DATE_FORMAT(a.AgendaBloqueadaInicio, '%d/%m/%Y %H:%i:%s') AS AgendaBloqueadaInicio,
                            DATE_FORMAT(a.AgendaBloqueadaFim, '%d/%m/%Y %H:%i:%s') AS AgendaBloqueadaFim,
                            a.IdAgendaSemanaInicio AS IdAgendaSemanaInicio,
                            a.IdAgendaSemanaFim AS IdAgendaSemanaFim,
                            CAST(c.UsuarioId AS CHAR) AS UsuarioId,
                            CAST(cc.ContaId AS CHAR) AS ContaId
                        FROM AgendaColaborador ac
                        INNER JOIN Agenda a ON a.Id = ac.IdAgenda
                        INNER JOIN Colaborador c ON c.Id = ac.IdColaborador
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id 
                        WHERE ac.IdColaborador = @IdColaborador
                          AND a.Status = 1
                        ORDER BY a.Id DESC";

            var response = await _dbAgenda.Connection.QueryAsync<AgendaDTO>(sql, new { IdColaborador = idColaborador })
                ?? Enumerable.Empty<AgendaDTO>();
            return response;
        }
    }
}
