using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class AgendaColaboradorDomainService : IAgendaContextDomainService<AgendaColaborador>, IAgendaColaboradorDomainService
    {
        private readonly IAgendaContextRepository<AgendaColaborador> _repository;
        private readonly IAgendaColaboradorRepository _agendaColaboradorRepository;

        public AgendaColaboradorDomainService(
            IAgendaContextRepository<AgendaColaborador> repository,
            IAgendaColaboradorRepository agendaColaboradorRepository)
        {
            _repository = repository;
            _agendaColaboradorRepository = agendaColaboradorRepository;
        }

        public async Task<PaginacaoDTO<AgendaDTO>> BuscarPaginado(PaginacaoDTO<AgendaDTO> paginacao, string contaId)
        {
            return await _agendaColaboradorRepository.BuscarPaginado(paginacao, contaId ?? string.Empty);
        }

        public async Task<AgendaDTO> BuscarPorId(int id)
        {
            if (id == 0)
                return new AgendaDTO();

            var response = await _repository.BuscarPorId(id);
            var dataFinalDia = ValoresHelper.SetDateTimeCustomer(response?.AgendaBloqueadaFim);
            response!.Bloqueado = dataFinalDia == DateTime.Today.AddDays(1).AddMinutes(-1);

            return response;
        }

        public async Task<AgendaDTO> BuscarPorIdColaborador(int idColaborador)
        {
            return await _agendaColaboradorRepository.BuscarPorIdColaborador(idColaborador);
        }

        public async Task<int> Persistir(AgendaColaborador agendaColaborador)
        {
            agendaColaborador.Status = true;
            agendaColaborador.DataAtualizacao = DateTime.Now;
            if (agendaColaborador.Id == 0)
            {
                agendaColaborador.DataCadastro = DateTime.Now;
                return await _repository.Salvar(agendaColaborador);
            }

            await _repository.Atualizar(agendaColaborador);
            return agendaColaborador.Id;
        }
    }
}
