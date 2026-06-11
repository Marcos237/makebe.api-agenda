using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Extensions;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.AgendamentoSpecifications
{
    public class AgendamentoLojaPeriodoSpecification : Specification<AgendamentoDTO>
    {
        private readonly IAgendamentoLojaRepository _agendamentoLojaRepository;
        public AgendamentoLojaPeriodoSpecification(IAgendamentoLojaRepository agendamentoLojaRepository)
        {
            _agendamentoLojaRepository = agendamentoLojaRepository;
        }
        public override bool IsSatisfiedBy(AgendamentoDTO item)
        {
            var id = Convert.ToInt32(item.IdColaborador ?? "0");
            var dataIncio = ValoresHelper.MontarDate(item?.DataInicioAgendamentoExtenso, item?.Data) ?? DateTime.Now;
            var dataFim = item.MontarDataTermino();
            var response = _agendamentoLojaRepository.BuscarAgendamentoLojaAgendaAberta(id, dataIncio, dataFim).Result;
            return !response.Any();
        }
    }
}
