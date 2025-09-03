using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.AgendamentoSpecifications
{
    public class AgendaLojaBloqueadaSpecification : Specification<AgendamentoDTO>
    {
        private readonly IAgendaContextRepository<AgendaLoja> _agendaContextRepository;
        public AgendaLojaBloqueadaSpecification(IAgendaContextRepository<AgendaLoja> agendaContextRepository)
        {
            _agendaContextRepository = agendaContextRepository;
        }
        public override bool IsSatisfiedBy(AgendamentoDTO item)
        {
            var response = _agendaContextRepository.BuscarAgendaLojaDentroDoBloqueio(item.DataInicioAgendamento, item.DataTerminoAgendamento, item.IdLoja).Result;
            return !response.Any();
        }
    }
}
