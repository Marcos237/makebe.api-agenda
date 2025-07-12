using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        private readonly DbAgenda _dbAgenda;
        public AgendamentoRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<PaginacaoDTO<AgendamentoDTO>> BuscarPaginado(PaginacaoDTO<AgendamentoDTO> paginacao, string contaId)
        {
            var sql = @"SELECT 
                        	ag.Id, 
                        	ag.IdAgendaColaborador,
                        	c.UsuarioId  AS IdColaborador,
                        	ag.IdServico,
                        	al.IdLoja,
                        	l.RazaoSocial, 
                        	s.Descricao As DescricaoServico,
                        	ag.IdUsuario, 
                        	ag.DataInicioAgendamento,
                        	ag.DataTerminoAgendamento
                        FROM Agendamento ag 
                        INNER JOIN AgendaColaborador ac  ON ac.Id  = ag.IdAgendaColaborador AND ac.Status  = 1
                        INNER JOIN Colaborador c ON c.id = ac.IdColaborador
                        INNER JOIN Agenda a ON a.Id  = ac.IdAgenda  AND a.Status  = 1
                        INNER JOIN AgendaLoja al On al.IdAgenda  = a.Id
                        INNER JOIN Loja l ON l.Id = al.IdLoja
                        INNER JOIN Servicos s ON s.Id  = ag.IdServico
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = ac.IdColaborador 
                        WHERE
                        cc.ContaId = @Conta
                        AND
                        ag.Ativo = 1";
            paginacao!.objetos = await _dbAgenda.Connection.QueryAsync<AgendamentoDTO>(sql, new { ContaId = contaId }) ?? Enumerable.Empty<AgendamentoDTO>();
            return paginacao;
        }

        public async Task<AgendamentoDTO> BuscarPorId(int id)
        {
            var sql = @"SELECT 
                        	ag.Id, 
                        	ag.IdAgendaColaborador,
                        	ag.IdServico,
                        	al.IdLoja,
                        	s.Descricao As DescricaoServico,
                        	ag.IdUsuario, 
                        	ag.DataInicioAgendamento,
                        	ag.DataTerminoAgendamento
                        FROM Agendamento ag 
                        INNER JOIN AgendaColaborador ac  ON ac.Id  = ag.IdAgendaColaborador AND ac.Status  = 1
                        INNER JOIN Agenda a ON a.Id  = ac.IdAgenda  AND a.Status  = 1
                        INNER JOIN AgendaLoja al On al.IdAgenda  = a.Id
                        INNER JOIN Servicos s ON s.Id  = ag.IdServico
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId  = ac.IdColaborador  
                        WHERE ag.Id = @Id
                        AND ag.Ativo = 1";

            var response = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<AgendamentoDTO>(sql, new { Id = id }) ?? new AgendamentoDTO();
            return response;
        }
        public async Task<int> Salvar(Agendamento agendamento)
        {
            var sql = @"INSERT INTO Agendamento (IdAgendaColaborador, IdServico, IdUsuario, DataInicioAgendamento , DataTerminoAgendamento , DataCadastro , DataAtualizacao, Ativo)
                                        VALUES (@IdAgendaColaborador, @IdServico, @IdUsuario, @DataInicioAgendamento , @DataTerminoAgendamento , @DataCadastro , @DataAtualizacao, @Ativo)";
            var response = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                IdAgendaColaborado = agendamento.IdAgendaColaborado,
                IdServico = agendamento.IdServico,
                IdUsuario = agendamento.IdUsuario,
                DataInicioAgendamento = agendamento.DataInicioAgendamento,
                DataTerminoAgendamento = agendamento.DataTerminoAgendamento,
                DataCadastro = agendamento.DataCadastro,
                DataAtualizacao = agendamento.DataAtualizacao,
                Ativo = agendamento.Ativo,
            });
            return response;
        }
        public async Task<bool> Atualizar(Agendamento agendamento)
        {
            var sql = @"UPDATE Agendamento
                           SET
                             IdServico = @IdServico,
                             IdUsuario = @IdUsuario,
                             DataInicioAgendamento = @DataInicioAgendamento,
                             DataTerminoAgendamento = @DataTerminoAgendamento,
                             DataAtualizacao = @DataAtualizacao,
                             Ativo = @Ativo,
                             IdAgendaColaborador = @IdAgendaColaborador
                       WHERE Id = @Id";
            var response = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                Id = agendamento.Id,
                IdAgendaColaborado = agendamento.IdAgendaColaborado,
                IdServico = agendamento.IdServico,
                IdUsuario = agendamento.IdUsuario,
                DataInicioAgendamento = agendamento.DataInicioAgendamento,
                DataTerminoAgendamento = agendamento.DataTerminoAgendamento,
                DataAtualizacao = agendamento.DataAtualizacao
            });
            return response > 0;
        }
        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE Agendamento a  SET Ativo = 0 WHERE Id = @Id
";
            var response = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                Id = id
            });
            return response > 0;
        }
    }
}
