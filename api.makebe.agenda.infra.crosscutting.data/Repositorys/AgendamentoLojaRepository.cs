using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class AgendamentoLojaRepository : IAgendamentoLojaRepository
    {
        private readonly DbAgenda _dbAgenda;
        public AgendamentoLojaRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }

        public async Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoLojaAgendaAberta(int colaboradorId, DateTime dataInicio, DateTime dataFim)
        {
            var sql = @" 
                        SELECT 
                        	a.id AS IdAgenda, 
                        	l.Id As IdLoja,
                        	l.RazaoSocial, 
                        	c.Id AS IdColaborador,
                        	a.AgendaAbertaInicio , 
                        	a.AgendaAbertaFim, 
                        	a.IsBloqueadoHoje, 
                        	a.AgendaBloqueadaInicio, 
                        	a.AgendaBloqueadaFim  
                        FROM Colaborador c
                        INNER JOIN AgendaColaborador ac ON ac.IdColaborador  = c.Id
                        INNER JOIN AgendaLoja al ON ac.IdAgenda = al.IdAgenda
                        INNER JOIN Agenda a ON a.Id = al.IdAgenda
                        INNER JOIN Loja l on l.Id  = al.IdLoja
                        WHERE 
                       c.Status  = 1 AND c.Id = @Id AND a.Status = 1
                         AND (
                                @DataInicio  BETWEEN a.AgendaAbertaInicio AND a.AgendaAbertaFim
                             OR @DataTermino BETWEEN a.AgendaAbertaInicio AND a.AgendaAbertaFim
                             OR a.AgendaAbertaInicio BETWEEN @DataInicio AND @DataTermino
                             OR a.AgendaAbertaFim     BETWEEN @DataInicio AND @DataTermino
                         );";
            var retorno = await _dbAgenda.Connection.QueryAsync<AgendamentoDTO>(sql, new
            {
                Id = colaboradorId,
                DataInicio = dataInicio,
                DataTermino = dataFim
            }) ?? Enumerable.Empty<AgendamentoDTO>();
            return retorno;
        }
        public async Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoLojaBloqueada(int colaboradorId, DateTime dataInicio, DateTime dataFim)
        {
            var sql = @" 
                     SELECT 
                        	a.id AS IdAgenda, 
                        	l.Id As IdLoja,
                        	l.RazaoSocial, 
                        	c.Id AS IdColaborador,
                        	a.AgendaAbertaInicio , 
                        	a.AgendaAbertaFim, 
                        	a.IsBloqueadoHoje, 
                        	a.AgendaBloqueadaInicio, 
                        	a.AgendaBloqueadaFim, 
                        	a.Status 
                        FROM Colaborador c
                        INNER JOIN ColaboradorProfissional cp ON cp.ColaboradorId  = c.id
                        INNER JOIN AgendaLoja al ON cp.LojaId  = al.IdLoja 
                        INNER JOIN Agenda a ON a.Id = al.IdAgenda
                        INNER JOIN Loja l on l.Id  = al.IdLoja
                        WHERE c.Status = 1 AND c.Id = @Id AND a.Status = 1
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
                Id = colaboradorId,
                DataInicio = dataInicio,
                DataTermino = dataFim
            }) ?? Enumerable.Empty<AgendamentoDTO>();
            return retorno;
        }
    }
}
