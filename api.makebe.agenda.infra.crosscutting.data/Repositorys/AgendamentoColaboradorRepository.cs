using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;
using static System.Net.Mime.MediaTypeNames;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class AgendamentoColaboradorRepository : IAgendamentoColaboradorRepository
    {
        private readonly DbAgenda _dbAgenda;
        public AgendamentoColaboradorRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoColaboradorAgendaBloqueada(int idColaborador, DateTime dataInicio, DateTime dataFim)
        {
            var sql = @" 
                        SELECT 
                            a.id AS IdAgenda, 
                            c.Id AS IdColaborador,
                            a.AgendaAbertaInicio, 
                            a.AgendaAbertaFim, 
                            a.IsBloqueadoHoje, 
                            a.AgendaBloqueadaInicio, 
                            a.AgendaBloqueadaFim,
                            c.Status 
                        FROM Colaborador c
                        JOIN AgendaColaborador ac ON ac.IdColaborador = c.Id
                        JOIN Agenda a             ON a.Id            = ac.IdAgenda
                        WHERE c.Status = 1 AND a.Status = 1
                          AND c.Id = @Id
                     AND (
                        
                               (a.IsBloqueadoHoje = 1 AND (
                                    @DataInicio  BETWEEN a.AgendaBloqueadaInicio AND a.AgendaBloqueadaFim
                                 OR @DataTermino BETWEEN a.AgendaBloqueadaInicio AND a.AgendaBloqueadaFim
                                 OR a.AgendaBloqueadaInicio BETWEEN @DataInicio AND @DataTermino
                                 OR a.AgendaBloqueadaFim     BETWEEN @DataInicio AND @DataTermino
                               ))
                        
                        OR (
                          a.IsBloqueadoHoje <> 1
                          AND (
                        
                            (TIME(a.AgendaBloqueadaFim) > TIME(a.AgendaBloqueadaInicio)
                             AND TIME(@DataInicio)  < TIME(a.AgendaBloqueadaFim)
                             AND TIME(@DataTermino) > TIME(a.AgendaBloqueadaInicio)
                            )
                        
                        
                            OR (TIME(a.AgendaBloqueadaFim) <= TIME(a.AgendaBloqueadaInicio) AND (
                        
                                 (TIME(@DataTermino) > TIME(a.AgendaBloqueadaInicio))
                        
                              OR (TIME(@DataInicio)  < TIME(a.AgendaBloqueadaFim))
                            ))
                          )
                        )
                        );";
            var retorno = await _dbAgenda.Connection.QueryAsync<AgendamentoDTO>(sql, new
            {
                Id = idColaborador,
                DataInicio = dataInicio,
                DataTermino = dataFim
            }) ?? Enumerable.Empty<AgendamentoDTO>();
            return retorno;
        }


        public async Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoColaboradorDatas(int idColaborador, DateTime dataInicio, DateTime dataFim)
        {
            var sql = @"
                        SELECT 
                            a.id AS IdAgenda, 
                            c.Id AS IdColaborador,
                            a.AgendaAbertaInicio, 
                            a.AgendaAbertaFim, 
                            a.IsBloqueadoHoje, 
                            a.AgendaBloqueadaInicio, 
                            a.AgendaBloqueadaFim,
                            c.Status 
                        FROM Colaborador c
                        JOIN AgendaColaborador ac ON ac.IdColaborador = c.Id
                        JOIN Agenda a             ON a.Id            = ac.IdAgenda
                        WHERE c.Status = 1 AND a.Status = 1
                          AND c.Id = @Id
                         AND (
                                @DataInicio  BETWEEN a.AgendaAbertaInicio AND a.AgendaAbertaFim
                             OR @DataTermino BETWEEN a.AgendaAbertaInicio AND a.AgendaAbertaFim
                             OR a.AgendaAbertaInicio BETWEEN @DataInicio AND @DataTermino
                             OR a.AgendaAbertaFim     BETWEEN @DataInicio AND @DataTermino
                         );";
            var retorno = await _dbAgenda.Connection.QueryAsync<AgendamentoDTO>(sql, new
            {
                Id = idColaborador,
                DataInicio = dataInicio,
                DataTermino = dataFim
            }) ?? Enumerable.Empty<AgendamentoDTO>();
            return retorno;
        }

        public async Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoColaboradorDisponivel(int idColaborador, DateTime dataInicio, DateTime dataFim, int idAgendamento)
        {
            var sql = @"SELECT 
                        	a.id,
                        	a.idAgendaColaborador,
                        	a.DataInicioAgendamento,
                        	a.DataTerminoAgendamento
 
                        FROM Colaborador c
                        INNER JOIN AgendaColaborador ac ON ac.IdColaborador  = c.Id
                        INNER JOIN Agendamento a ON a.IdAgendaColaborador  = ac.Id

                        WHERE 
                        a.Ativo  = 1 AND c.Id = @Id AND c.Status = 1 AND a.Id <> @IdAgendamento
                         AND (
                                @DataInicio  BETWEEN a.DataInicioAgendamento AND a.DataTerminoAgendamento
                             OR @DataTermino BETWEEN a.DataInicioAgendamento AND a.DataTerminoAgendamento
                             OR a.DataInicioAgendamento BETWEEN @DataInicio AND @DataTermino
                             OR a.DataTerminoAgendamento     BETWEEN @DataInicio AND @DataTermino
                         );
                        ";
            var retorno = await _dbAgenda.Connection.QueryAsync<AgendamentoDTO>(sql, new
            {
                Id = idColaborador,
                DataInicio = dataInicio,
                DataTermino = dataFim,
                IdAgendamento = idAgendamento
            }) ?? Enumerable.Empty<AgendamentoDTO>();
            return retorno;
        }

        public async Task<IEnumerable<ColaboradorDTO>> BuscarAgendamentoColaboradores(string conta)
        {
            var sql = @" SELECT c.* FROM Colaborador c
                         INNER JOIN  AgendaColaborador ac ON ac.IdColaborador  = c.Id
                         INNER JOIN Agenda a ON a.Id  = ac.IdAgenda
                         INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = c.Id
                         WHERE
                         cc.ContaId  = @ContaId
						 AND
                         a.Status = 1";
            var retorno = await _dbAgenda.Connection.QueryAsync<ColaboradorDTO>(sql, new { ContaId = conta }) ?? Enumerable.Empty<ColaboradorDTO>();
            return retorno;
        }

        public async Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentosPorColaboradorId(int idColaborador)
        {
            var sql = @"SELECT 
                            a.Id, 
                            a.IdAgendaColaborador,
                            a.IdServico,
                            CAST(a.IdUsuario AS CHAR) AS IdUsuario,
                            a.DataInicioAgendamento,
                            a.DataTerminoAgendamento,
                            c.Id AS IdColaborador,
                            s.Descricao AS DescricaoServico,                   
                            s.Valor,
                            s.Periodo
                        FROM Agendamento a
                        INNER JOIN AgendaColaborador ac ON ac.Id = a.IdAgendaColaborador
                        INNER JOIN Colaborador c ON c.Id = ac.IdColaborador
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId = c.Id
                        INNER JOIN Servicos s ON s.Id = a.IdServico
                        WHERE c.Id = @IdColaborador
                          AND a.Ativo = 1
                        ORDER BY a.DataInicioAgendamento DESC";

            var retorno = await _dbAgenda.Connection.QueryAsync<AgendamentoDTO>(sql, new { IdColaborador = idColaborador })
                ?? Enumerable.Empty<AgendamentoDTO>();
            return retorno;
        }

        public async Task<IEnumerable<AgendamentoColaboradorPeriodoDTO>> BuscarPeriodosPorColaboradorId(int idColaborador)
        {
            var sql = @"SELECT DISTINCT
                            ac.Id AS IdAgendaColaborador,
                            cp.ColaboradorId,
                            cp.PeriodoInativoInicio,
                            cp.PeriodoInativoFim,
                            a.AgendaBloqueadaInicio,
                            a.AgendaBloqueadaFim,
                            ag.DataInicioAgendamento,
                            ag.DataTerminoAgendamento,
                            s.Periodo
                        FROM ColaboradorProfissional cp
                        LEFT JOIN AgendaColaborador ac
                            ON ac.IdColaborador = cp.ColaboradorId
                        LEFT JOIN Agenda a
                            ON a.Id = ac.IdAgenda
                        LEFT JOIN Agendamento ag
                            ON ag.IdAgendaColaborador = ac.Id
                            AND ag.Ativo = 1
                        LEFT JOIN Servicos s
                            ON s.Id = ag.IdServico
                        WHERE
                            cp.Status = 1
                            AND ac.Status = 1
                        AND ac.IdColaborador = @IdColaborador";

            var retorno = await _dbAgenda.Connection.QueryAsync<AgendamentoColaboradorPeriodoDTO>(sql, new { IdColaborador = idColaborador })
                ?? Enumerable.Empty<AgendamentoColaboradorPeriodoDTO>();
            return retorno;
        }
    }
}
