using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class AgendamentoColaboradorDomainServices : IAgendamentoColaboradorDomainService
    {
        private readonly IAgendamentoColaboradorRepository _agendamentoColaboradorRepository;
        public AgendamentoColaboradorDomainServices(IAgendamentoColaboradorRepository agendamentoColaboradorRepository)
        {
            _agendamentoColaboradorRepository = agendamentoColaboradorRepository;
        }
        public async Task<IEnumerable<ColaboradorDTO>> MontarColaboradores(IEnumerable<UsuarioDTO>? usuarios, string contaId)
        {
            var colaboradores = (await _agendamentoColaboradorRepository.BuscarAgendamentoColaboradores(contaId));

            var colaboradoresFiltrados = usuarios?.Where(usuario => colaboradores.Any(colaborador =>
                    colaborador.UsuarioId == PropiedadesHelper.ParseGuidOrDefault(usuario.Id)))
                .Select(usuario => new ColaboradorDTO
                {
                    Id = colaboradores
                        .FirstOrDefault(colaborador =>
                            colaborador.UsuarioId == PropiedadesHelper.ParseGuidOrDefault(usuario.Id))?.Id ?? 0,
                    Nome = usuario.Nome,
                    UsuarioId = PropiedadesHelper.ParseGuidOrDefault(usuario.Id),
                    UrlImagem = usuario.UrlImagem
                }) ?? Enumerable.Empty<ColaboradorDTO>();

            return colaboradoresFiltrados;
        }
    }
}

