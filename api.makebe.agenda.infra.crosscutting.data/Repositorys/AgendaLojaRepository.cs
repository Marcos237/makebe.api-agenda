using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class AgendaLojaRepository : IAgendaContextRepository<AgendaLoja>
    {
        private readonly DbAgenda _dbAgenda;
        public AgendaLojaRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<PaginacaoDTO<AgendaDTO>> BuscarPaginado(PaginacaoDTO<AgendaDTO> paginacao, string contaId)
        {

            paginacao.registroInicial = (paginacao.paginaAtual - 1) * paginacao.quantidadePagina;
            var sql = await BuscarConsulta();
            var parametros = new
            {
                RegistroInicial = paginacao.registroInicial,
                TamanhoPagina = paginacao.quantidadePagina,
                RazaoSocial = paginacao?.objetoPesquisa?.RazaoSocial,
                IdAgendaSemanaInicio = paginacao?.objetoPesquisa?.IdAgendaSemanaInicio,
                IdAgendaSemanaFim = paginacao?.objetoPesquisa?.IdAgendaSemanaFim,
                AgendaAbertaInicio = string.IsNullOrWhiteSpace(paginacao?.objetoPesquisa?.AgendaAbertaInicio) ? null : paginacao?.objetoPesquisa?.AgendaAbertaInicio,
                AgendaAbertaFim = string.IsNullOrWhiteSpace(paginacao?.objetoPesquisa?.AgendaAbertaFim) ? null : paginacao?.objetoPesquisa?.AgendaAbertaFim,
                ContaId = contaId,
                Bloqueado = paginacao?.objetoPesquisa?.Bloqueado,
                IsTodoDia = paginacao?.objetoPesquisa?.IsTodoDia,
                AgendaBloqueadaInicio = string.IsNullOrWhiteSpace(paginacao?.objetoPesquisa?.AgendaBloqueadaInicio) ? null : paginacao?.objetoPesquisa?.AgendaBloqueadaInicio,
                AgendaBloqueadaFim = string.IsNullOrWhiteSpace(paginacao?.objetoPesquisa?.AgendaBloqueadaFim) ? null : paginacao?.objetoPesquisa?.AgendaBloqueadaFim,
            };
            var agendas = await _dbAgenda.Connection.QueryAsync<AgendaDTO>(sql, parametros) ?? Enumerable.Empty<AgendaDTO>();
            paginacao!.total = agendas.Count();
            string sqlBusca = $"{sql} LIMIT @TamanhoPagina OFFSET @RegistroInicial";
            paginacao!.objetos = await _dbAgenda.Connection.QueryAsync<AgendaDTO>(sqlBusca, parametros) ?? Enumerable.Empty<AgendaDTO>();
            return paginacao;
        }

        public async Task<AgendaDTO> BuscarPorId(int id)
        {
            var sql = @"SELECT 
                         a.id,
                         l.Id As IdLoja,  
                         l.RazaoSocial, 
                         al.Bloqueado, 
                         a.IsTodoDia,
                         a.IdAgendaSemanaInicio ,
                         a.IdAgendaSemanaFim,
                         DATE_FORMAT(a.AgendaAbertaInicio, '%d/%m/%Y %H:%i:%s') AS AgendaAbertaInicio, 
                         DATE_FORMAT(a.AgendaAbertaFim, '%d/%m/%Y %H:%i:%s') AS AgendaAbertaFim,
                         a.IsBloqueadoHoje,
                         DATE_FORMAT(a.AgendaBloqueadaInicio, '%d/%m/%Y %H:%i:%s') AS AgendaBloqueadaInicio,
                         DATE_FORMAT(a.AgendaBloqueadaFim, '%d/%m/%Y %H:%i:%s') AS AgendaBloqueadaFim,
                         a.Status
                        FROM Loja l  
                        INNER JOIN ContaLoja cl ON cl.LojaId  = l.Id
                        INNER JOIN AgendaLoja al ON al.IdLoja  = l.Id
                        INNER JOIN Agenda a  ON a.Id  = al.IdAgenda
                        WHERE  a.Status  = 1
                        AND a.Id = @Id";
            var response = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<AgendaDTO>(sql, new { Id = id }) ?? new AgendaDTO();
            return response;
        }
        public async Task<IEnumerable<AgendaDTO>> BuscarAgendaLojaDentroDoBloqueio(DateTime dataInicio, DateTime DataFim, int idLoja)
        {
            var sql = @"SELECT a.*, al.IdLoja
                       FROM AgendaLoja al
                       INNER JOIN Agenda a ON a.Id = al.IdAgenda
                       WHERE 
                       	a.AgendaBloqueadaInicio >= @DataInicioAgendamento 
                         AND a.AgendaBloqueadaFim <= @DataTerminoAgendamento
                         AND a.Status = 1 
                         AND al.IdLoja = @LojaId ;";
            var response = await _dbAgenda.Connection.QueryAsync<AgendaDTO>(sql, new
            {
                DataInicioAgendamento = dataInicio,
                DataTerminoAgendamento = DataFim,
                IdLoja = idLoja
            });
            return response;
        }

        public async Task<int> Salvar(AgendaLoja agendaLoja)
        {
            var sql = @"INSERT INTO AgendaLoja (IdAgenda, IdLoja, Bloqueado, DataCadastro, DataAtualizacao , Status)
                         VALUES (@IdAgenda, @IdLoja, @Bloqueado, @DataCadastro, @DataAtualizacao , @Status);
                        SELECT LAST_INSERT_ID();";
            var response = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                IdAgenda = agendaLoja.IdAgenda,
                IdLoja = agendaLoja.IdLoja,
                Bloqueado = agendaLoja.Bloqueado,
                DataCadastro = agendaLoja.DataCadastro,
                DataAtualizacao = agendaLoja.DataAtualizacao,
                Status = agendaLoja.Status
            }, _dbAgenda.Transaction);
            return response;
        }

        public async Task<bool> Atualizar(AgendaLoja agendaLoja)
        {
            var sql = @"
                        UPDATE AgendaLoja
                        SET 
                            Bloqueado = @Bloqueado,
                            DataAtualizacao = @DataAtualizacao
                        WHERE Id = @Id";

            var response = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                Bloqueado = agendaLoja.Bloqueado,
                DataAtualizacao = agendaLoja.DataAtualizacao,
                Id = agendaLoja.Id
            }) > 0;
            return response;
        }
        private Task<string> BuscarConsulta()
        {
            var query = @"SELECT 
                        a.id,
                        l.Id AS IdLoja,
                        l.RazaoSocial,
                        al.Bloqueado,
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
                    FROM 
                        Loja l
                    INNER JOIN 
                        ContaLoja cl ON cl.LojaId = l.Id
                    INNER JOIN 
                        AgendaLoja al ON al.IdLoja = l.Id
                    INNER JOIN 
                        Agenda a ON a.Id = al.IdAgenda
                    INNER JOIN 
                        AgendaSemana as2 ON as2.Id = a.IdAgendaSemanaInicio
                    INNER JOIN 
                        AgendaSemana as3 ON as3.Id = a.IdAgendaSemanaFim
                    WHERE 
                        cl.ContaId = @ContaId
                        AND a.Status = 1
                        AND (
                            @RazaoSocial IS NULL
                            OR @RazaoSocial = ''
                            OR l.RazaoSocial LIKE CONCAT('%', @RazaoSocial, '%')
                        )
                        AND (
                            (
                                (@IdAgendaSemanaInicio IS NULL OR @IdAgendaSemanaInicio = 0)
                                AND (@IdAgendaSemanaFim IS NULL OR @IdAgendaSemanaFim = 0)
                            )
                            OR (
                                a.IdAgendaSemanaInicio >= @IdAgendaSemanaInicio
                                AND a.IdAgendaSemanaInicio <= @IdAgendaSemanaFim
                            )
                        )
                        AND (
                            (
                               (@AgendaAbertaInicio IS NOT NULL AND @AgendaAbertaFim IS NOT NULL AND @AgendaAbertaInicio != '' AND @AgendaAbertaFim != '')
                                AND a.AgendaAbertaInicio >=  STR_TO_DATE(@AgendaAbertaInicio, '%d/%m/%Y %H:%i:%s') 
                                AND a.AgendaAbertaFim <= STR_TO_DATE(@AgendaAbertaFim, '%d/%m/%Y %H:%i:%s')
                            )
                           OR (
                                (@AgendaAbertaInicio IS NOT NULL OR @AgendaAbertaInicio != '')
                                AND DATE(a.AgendaAbertaInicio) = STR_TO_DATE(@AgendaAbertaInicio, '%d/%m/%Y')
                                )
                           OR (
                                (@AgendaAbertaFim IS NOT NULL OR @AgendaAbertaFim != '')
                                AND DATE(a.AgendaAbertaFim) = STR_TO_DATE(@AgendaAbertaFim, '%d/%m/%Y')
                               )
                           OR (
                                (@AgendaAbertaInicio IS NULL OR @AgendaAbertaInicio = '')
                                AND (@AgendaAbertaFim IS NULL OR @AgendaAbertaFim = '')
                               )
                          )
                        
                        AND (
                                (
                    (@AgendaBloqueadaInicio IS NOT NULL AND @AgendaBloqueadaInicio != '' AND @AgendaBloqueadaFim IS NOT NULL AND @AgendaBloqueadaFim != '')
                    AND TIME(a.AgendaBloqueadaInicio) >= TIME(STR_TO_DATE(@AgendaBloqueadaInicio, '%d/%m/%Y %H:%i'))
                    AND TIME(
                        IF(
                            TIME(a.AgendaBloqueadaFim) = '00:00:00',
                            DATE_SUB(a.AgendaBloqueadaFim, INTERVAL 1 MINUTE),
                            a.AgendaBloqueadaFim
                        )
                    ) <= TIME(STR_TO_DATE(@AgendaBloqueadaFim, '%d/%m/%Y %H:%i'))
                                )
                            OR (
                                    @AgendaBloqueadaInicio IS NOT NULL AND @AgendaBloqueadaInicio != ''
                                    AND DATE_FORMAT(a.AgendaBloqueadaInicio, '%H:%i') = DATE_FORMAT(STR_TO_DATE(@AgendaBloqueadaInicio, '%d/%m/%Y %H:%i:%s'), '%H:%i')
                                )
                                OR (
                                    @AgendaBloqueadaFim IS NOT NULL AND @AgendaBloqueadaFim != ''
                                    AND DATE_FORMAT(a.AgendaBloqueadaFim, '%H:%i') = DATE_FORMAT(STR_TO_DATE(@AgendaBloqueadaFim, '%d/%m/%Y %H:%i:%s'), '%H:%i')
                                )
                            OR (
                                (@AgendaBloqueadaInicio IS NULL OR @AgendaBloqueadaInicio = '') AND (@AgendaBloqueadaFim IS NULL OR @AgendaBloqueadaFim = '')
                            )
                        )                                                      
        ";
            return Task.FromResult(query);
        }
    }
}
