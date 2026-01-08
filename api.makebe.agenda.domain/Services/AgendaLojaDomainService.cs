using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class AgendaLojaDomainService : IAgendaContextDomainService<AgendaLoja>
    {
        private readonly IAgendaContextRepository<AgendaLoja> _agendaLojaRepository;
        public AgendaLojaDomainService(IAgendaContextRepository<AgendaLoja> agendaLojaRepository)
        {
            _agendaLojaRepository = agendaLojaRepository;   
        }
        public async Task<PaginacaoDTO<AgendaDTO>> BuscarPaginado(PaginacaoDTO<AgendaDTO> paginacao, string contaId)
        {
            var response = await _agendaLojaRepository.BuscarPaginado(paginacao, contaId);
            response.totalPaginas = (response.total + response.quantidadePagina - 1) / response.quantidadePagina;
            return response;
        }

        public async Task<AgendaDTO> BuscarPorId(int id)
        {
            var response = await _agendaLojaRepository.BuscarPorId(id);
            if (!string.IsNullOrEmpty(response.AgendaBloqueadaFim))
            {
                var horaFim = ValoresHelper.SetDateTimeCustomer(response?.AgendaBloqueadaFim)!.Value;
                var horaLimite = DateTime.Today;
                var horaFimHM = new TimeSpan(horaFim.Hour, horaFim.Minute, 0);
                var horaLimiteHM = new TimeSpan(horaLimite.Hour, horaLimite.Minute, 0);
                response!.Bloqueado = horaFimHM == horaLimiteHM;
            }
            return response!;
        }

        public async Task<int> Persistir(AgendaLoja agendaLoja)
        {
            agendaLoja.Status = true;
            agendaLoja.DataAtualizacao = DateTime.Now;
            if (agendaLoja.Id == 0)
            {
                agendaLoja.DataCadastro = DateTime.Now;
                var result = await _agendaLojaRepository.Salvar(agendaLoja);
                return result;
            }
            var resultUpdate = await _agendaLojaRepository.Atualizar(agendaLoja);
            return agendaLoja.Id;
        }

    }
}
