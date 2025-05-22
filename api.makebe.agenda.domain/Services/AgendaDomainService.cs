using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class AgendaDomainService : IAgendaDomainService
    {
        private readonly IAgendaRepository _agendaRepository;
        public AgendaDomainService(IAgendaRepository agendaRepository)
        {
            _agendaRepository = agendaRepository;   
        }

        public async Task<int> Persitir(Agenda agenda)
        {
            agenda.DataAtualizacao = DateTime.Now;
            agenda.Status = true;
            await PreencherDiasSemana(agenda);
            await BloquearAgendaHoje(agenda);
            if (agenda.Id == 0)
            {
                agenda.DataCadastro = DateTime.Now;
                var response = await _agendaRepository.Salvar(agenda);
                return response;
            }
            var resposeAtualiza = await _agendaRepository.Atualizar(agenda);
            return agenda.Id;
        }

        public async Task<bool> Desativar(int id)
        {
            return await _agendaRepository.Desativar(id);
        }

        public async Task PreencherDiasSemana(Agenda agenda)
        {
            if (agenda.IsTodoDia)
            {
                await Task.FromResult(agenda.IdAgendaSemanaInicio = 1);
                await Task.FromResult(agenda.IdAgendaSemanaFim = 7);
            }
            
        }

        public async Task BloquearAgendaHoje(Agenda agenda)
        {
            if (agenda.IsBloqueadoHoje && (agenda.AgendaBloqueadaInicio == null && agenda.AgendaBloqueadaFim == null))
            {
                await Task.FromResult(agenda.AgendaBloqueadaInicio = DateTime.Now);
                agenda.AgendaBloqueadaFim = DateTime.Today.AddDays(1).AddMinutes(-1);

            }
        }
    }
}
