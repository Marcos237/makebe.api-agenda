using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
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
                        	CAST(c.IdUsuario AS CHAR) AS IdUsuario,
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
                        	a.Id as IdLoja,
                        	s.Descricao As DescricaoServico,
                        	CAST(ag.IdUsuario AS CHAR) AS IdUsuario,
                        	ag.DataInicioAgendamento,
                        	ag.DataTerminoAgendamento,
                        	c.Id AS IdColaborador,
                        	u.Nome As NomeCliente,
                            u.Telefone As TelefoneCliente 
                        FROM Agendamento ag 
                        INNER JOIN AgendaColaborador ac  ON ac.Id  = ag.IdAgendaColaborador AND ac.Status  = 1
                        INNER JOIN Agenda a ON a.Id  = ac.IdAgenda  AND a.Status  = 1
                        INNER JOIN Servicos s ON s.Id  = ag.IdServico
                        INNER JOIN Colaborador c ON c.Id  = ac.IdColaborador 
                        LEFT JOIN `Makebe.Sessao`.Usuario u ON u.Id = ag.IdUsuario
                        WHERE ag.Id = @Id
                        AND ag.Ativo = 1
                        ORDER BY ag.DataInicioAgendamento  DESC";

            var response = await _dbAgenda.Connection.QueryFirstOrDefaultAsync<AgendamentoDTO>(sql, new { Id = id }) ?? new AgendamentoDTO();
            return response;
        }

        public async Task<IEnumerable<AgendamentoConsultaDTO>> BuscarMeusAgendamentos(string idUsuario)
        {
            var sql = @"SELECT a.Id,
                            CAST(a.IdUsuario AS CHAR) AS IdUsuario,
                            a.DataInicioAgendamento,
                            a.DataTerminoAgendamento,
                            s.Descricao AS DescricaoServico,
                            ac.IdColaborador,
                            CAST(c.UsuarioId AS CHAR) AS IdColaboradorUsuario
                        FROM Agendamento a
                        INNER JOIN AgendaColaborador ac ON ac.Id = a.IdAgendaColaborador
                        INNER JOIN Servicos s ON s.Id = a.IdServico
                        INNER JOIN Colaborador c ON c.Id = ac.IdColaborador
                        WHERE 
                        a.IdUsuario = @IdUsuario 
                        AND 
                        a.Ativo = 1
                        ORDER BY a.DataInicioAgendamento  DESC";

            return await _dbAgenda.Connection.QueryAsync<AgendamentoConsultaDTO>(sql, new
            {
                IdUsuario = PropiedadesHelper.ParseGuidOrDefault(idUsuario)
            }) ?? Enumerable.Empty<AgendamentoConsultaDTO>();
        }

        public async Task<IEnumerable<AgendamentoDTO>> BuscarPorAnoConta(int ano, int id, string conta)
        {

            var sql = @"SELECT 
                        	a.Id, 
                        	a.IdAgendaColaborador,
                        	a.IdServico,
                            CAST(a.IdUsuario AS CHAR) AS IdUsuario,
                        	a.DataInicioAgendamento,
                        	a.DataTerminoAgendamento,
                            c.Id AS IdColaborador
                        FROM Agendamento a
                        INNER JOIN AgendaColaborador ac  ON ac.Id = a.IdAgendaColaborador
                        INNER JOIN Colaborador c ON c.Id  = ac.IdColaborador 
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId = c.Id
                        WHERE YEAR(a.DataInicioAgendamento) = @Ano
                        AND c.Id  = @ColaboradorId
                        AND cc.ContaId  = @Conta
                        AND a.Ativo = 1
                        ORDER BY a.DataInicioAgendamento  DESC";

            var response = await _dbAgenda.Connection.QueryAsync<AgendamentoDTO>(sql, new { Ano = ano, ColaboradorId = id, Conta = conta })
                ?? Enumerable.Empty<AgendamentoDTO>();
            return response;
        }

        public async Task<IEnumerable<AgendamentoDTO>> BuscarAgendamentoPorData(DateTime data, int id, string conta)
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
                            u.Nome As NomeCliente,
                            u.Telefone As TelefoneCliente 
                        FROM Agendamento a
                        INNER JOIN AgendaColaborador ac  ON ac.Id = a.IdAgendaColaborador
                        INNER JOIN Colaborador c ON c.Id  = ac.IdColaborador 
                        INNER JOIN ContaColaborador cc ON cc.ColaboradorId = c.Id
                        INNER JOIN Servicos s ON s.Id  = a.IdServico
                        LEFT JOIN `Makebe.Sessao`.Usuario u ON u.Id = a.IdUsuario
                        WHERE DATE(a.DataInicioAgendamento) = @Data
                        AND cc.ContaId  = @Conta
                        AND c.Id  = @ColaboradorId
                        AND a.Ativo = 1
                        ORDER BY a.DataInicioAgendamento  DESC";

            var response = await _dbAgenda.Connection.QueryAsync<AgendamentoDTO>(sql, new { Data = data, ColaboradorId = id, Conta = conta })
                ?? Enumerable.Empty<AgendamentoDTO>();
            return response;
        }

        public async Task<int> Salvar(Agendamento agendamento)
        {
            var sql = @"
        INSERT INTO Agendamento
        (
            IdAgendaColaborador,
            IdServico,
            IdUsuario,
            DataInicioAgendamento,
            DataTerminoAgendamento,
            DataCadastro,
            DataAtualizacao,
            Ativo
        )
        VALUES
        (
            @IdAgendaColaborador,
            @IdServico,
            @IdUsuario,
            @DataInicioAgendamento,
            @DataTerminoAgendamento,
            @DataCadastro,
            @DataAtualizacao,
            @Ativo
        );

        SELECT LAST_INSERT_ID();";

            return await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                IdAgendaColaborador = agendamento.IdAgendaColaborador,
                IdServico = agendamento.IdServico,
                IdUsuario = agendamento.IdUsuario,
                DataInicioAgendamento = agendamento.DataInicioAgendamento,
                DataTerminoAgendamento = agendamento.DataTerminoAgendamento,
                DataCadastro = agendamento.DataCadastro,
                DataAtualizacao = agendamento.DataAtualizacao,
                Ativo = agendamento.Ativo
            });
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
                IdAgendaColaborador = agendamento.IdAgendaColaborador,
                IdServico = agendamento.IdServico,
                IdUsuario = agendamento.IdUsuario,
                DataInicioAgendamento = agendamento.DataInicioAgendamento,
                DataTerminoAgendamento = agendamento.DataTerminoAgendamento,
                DataAtualizacao = agendamento.DataAtualizacao,
                Ativo = agendamento.Ativo
            });
            return response > 0;
        }
        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE Agendamento a  SET Ativo = 0 WHERE Id = @Id";
            var response = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                Id = id
            });
            return response > 0;
        }
    }
}
