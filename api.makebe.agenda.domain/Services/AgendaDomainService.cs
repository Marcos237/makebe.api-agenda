using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;
using System.Globalization;

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
            if (agenda.Id == 0)
            {
                agenda.DataCadastro = DateTime.Now;
                var response = await _agendaRepository.Salvar(agenda);
                return response;
            }
            if (agenda.IsBloqueadoHoje)
            {
                var ptBR = CultureInfo.GetCultureInfo("pt-BR");
                string dataInicio = agenda.AgendaBloqueadaInicio?.ToString("dd/MM/yyyy HH:mm", ptBR) ?? "";
                string dataFim = agenda.AgendaBloqueadaFim?.ToString("dd/MM/yyyy HH:mm", ptBR) ?? "";
                var hoje = DateTime.Now.ToString("dd/MM/yyyy", ptBR);
                agenda.AgendaBloqueadaInicio = ValoresHelper.MontarDate(dataInicio, hoje);
                agenda.AgendaBloqueadaFim = ValoresHelper.MontarDate(dataFim, hoje);
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
    }
}
