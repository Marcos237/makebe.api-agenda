using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.infra.crosscutting.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IAgendaColaboradorDomainService
    {
        Task<PaginacaoDTO<AgendaDTO>> Filtrar(PaginacaoDTO<AgendaDTO> paginacao);
        Task<PaginacaoDTO<AgendaDTO>> MontarColaboradorProfissional(PaginacaoDTO<AgendaDTO> paginacao, IEnumerable<UsuarioEvent>? UsuariosEvents);
        Task<IEnumerable<AgendaDTO>> FiltrarPorDiaSemana(PaginacaoDTO<AgendaDTO> paginacao, AgendaDTO? pesquisa);
        Task<IEnumerable<AgendaDTO>> PesquisarPorAgendaAberta(AgendaDTO agenda, IEnumerable<AgendaDTO> agendas);
        Task<IEnumerable<AgendaDTO>> PesquisarPorBloqueio(AgendaDTO agenda, IEnumerable<AgendaDTO> agendas);

    }
}
