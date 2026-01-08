using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using Dapper;

namespace api.makebe.agenda.infra.data.Repositorys
{
    public class AgendaRepository : IAgendaRepository
    {
        private readonly DbAgenda _dbAgenda;
        public AgendaRepository(DbAgenda dbAgenda)
        {
            _dbAgenda = dbAgenda;
        }
        public async Task<Agenda> BuscarPoId(int id)
        {
            var sql = @"SELECT * FROM Agenda Where Status = 1 AND Id = @Id";
            var response = await _dbAgenda.Connection.QueryFirstOrDefaultAsync(sql, new { Id = id });
            return response;
        }
        public async Task<int> Salvar(Agenda agenda)
        {
            var sql = @"INSERT INTO Agenda (IsTodoDia, IdAgendaSemanaInicio, IdAgendaSemanaFim, AgendaAbertaInicio, AgendaAbertaFim,  IsBloqueadoHoje, AgendaBloqueadaInicio, 
                                            AgendaBloqueadaFim, DataCadastro, DataAtualizacao, Status )
                        VALUES (@IsTodoDia, @IdAgendaSemanaInicio, @IdAgendaSemanaFim, @AgendaAbertaInicio, @AgendaAbertaFim,  @IsBloqueadoHoje, @AgendaBloqueadaInicio, 
                               @AgendaBloqueadaFim, @DataCadastro, @DataAtualizacao, @Status);
                                SELECT LAST_INSERT_ID();";
            var response = await _dbAgenda.Connection.ExecuteScalarAsync<int>(sql, new
            {
                IsTodoDia = agenda.IsTodoDia,
                IdAgendaSemanaInicio = agenda.IdAgendaSemanaInicio,
                IdAgendaSemanaFim = agenda.IdAgendaSemanaFim,
                AgendaAbertaInicio = agenda.AgendaAbertaInicio,
                AgendaAbertaFim = agenda.AgendaAbertaFim,
                IsBloqueadoHoje = agenda.IsBloqueadoHoje,
                AgendaBloqueadaInicio = agenda.AgendaBloqueadaInicio,
                AgendaBloqueadaFim = agenda.AgendaBloqueadaFim,
                DataCadastro = agenda.DataCadastro,
                DataAtualizacao = agenda.DataAtualizacao,
                Status = agenda.Status

            }, _dbAgenda.Transaction);
            return response;
        }
        public async Task<bool> Atualizar(Agenda agenda)
        {
            var sql = @"
                        UPDATE Agenda 
                        SET 
                            IsTodoDia = @IsTodoDia,
                            IdAgendaSemanaInicio = @IdAgendaSemanaInicio,
                            IdAgendaSemanaFim = @IdAgendaSemanaFim,
                            AgendaAbertaInicio = @AgendaAbertaInicio,
                            AgendaAbertaFim = @AgendaAbertaFim,
                            IsBloqueadoHoje = @IsBloqueadoHoje,
                            AgendaBloqueadaInicio = @AgendaBloqueadaInicio,
                            AgendaBloqueadaFim = @AgendaBloqueadaFim,
                            DataAtualizacao = @DataAtualizacao,
                            Status = @Status
                        WHERE Id = @Id";

            var response = await _dbAgenda.Connection.ExecuteAsync(sql, new
            {
                IsTodoDia = agenda.IsTodoDia,
                IdAgendaSemanaInicio = agenda.IdAgendaSemanaInicio,
                IdAgendaSemanaFim = agenda.IdAgendaSemanaFim,
                AgendaAbertaInicio = agenda.AgendaAbertaInicio,
                AgendaAbertaFim = agenda.AgendaAbertaFim,
                IsBloqueadoHoje = agenda.IsBloqueadoHoje,
                AgendaBloqueadaInicio = agenda.AgendaBloqueadaInicio,
                AgendaBloqueadaFim = agenda.AgendaBloqueadaFim,
                DataAtualizacao = agenda.DataAtualizacao,
                Status = agenda.Status,
                Id = agenda.Id

            }, _dbAgenda.Transaction) > 0;
            return response;
        }

        public async Task<bool> Desativar(int id)
        {
            var sql = @"UPDATE Agenda SET 
                      Status = 0 
                      Where Id = @Id";
            var retorno = await _dbAgenda.Connection.ExecuteAsync(sql, new
            { Id = id }) > 0;
            return retorno;
        }
    }
}
