using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Extensions;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.AgendamentoSpecifications
{
    public class AgendamentoDisponivelSpecification : Specification<AgendamentoDTO>
    {
        private readonly IAgendamentoColaboradorRepository _agendamentoColaboradorRepository;
        public AgendamentoDisponivelSpecification(IAgendamentoColaboradorRepository agendamentoColaboradorRepository)
        {
            _agendamentoColaboradorRepository = agendamentoColaboradorRepository;
        }
        public override bool IsSatisfiedBy(AgendamentoDTO item)
        {
            var id = Convert.ToInt32(item.IdColaborador ?? "0");
            var dataIncio = ValoresHelper.MontarDate(item?.DataInicioAgendamentoExtenso, item?.Data) ?? DateTime.Now;
            var dataFim = item.MontarDataTermino();
            var response = _agendamentoColaboradorRepository.BuscarAgendamentoColaboradorDisponivel(id, dataIncio, dataFim, item?.Id ?? 0).Result;
            return !response.Any();
        }
    }
}
