using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Extensions;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Specifications.SpecificationContext;

namespace api.makebe.agenda.domain.Specifications.AgendamentoSpecifications
{
    public class AgendamentoColaboradorPeriodoInativoSpecification : Specification<AgendamentoDTO>
    {
        private readonly IAgendamentoColaboradorRepository _agendamentoColaboradorRepository;

        public AgendamentoColaboradorPeriodoInativoSpecification(IAgendamentoColaboradorRepository agendamentoColaboradorRepository)
        {
            _agendamentoColaboradorRepository = agendamentoColaboradorRepository;
        }

        public override bool IsSatisfiedBy(AgendamentoDTO item)
        {
            var id = Convert.ToInt32(item.IdColaborador ?? "0");
            var dataInicio = ValoresHelper.MontarDate(item?.DataInicioAgendamentoExtenso, item?.Data) ?? DateTime.Now;
            var dataFim = item.MontarDataTermino();

            var dataPesquisaIncio = dataInicio.AddMinutes(1);
            var dataFimPesquisa = dataFim.AddMinutes(-1);

            var periodos = _agendamentoColaboradorRepository.BuscarPeriodosPorColaboradorId(id).Result;
            return !periodos.Any(periodo => EstaDentroPeriodoInativo(dataPesquisaIncio, dataFimPesquisa, periodo.PeriodoInativoInicio, periodo.PeriodoInativoFim));
        }

        private static bool EstaDentroPeriodoInativo(DateTime dataInicioAgendamento, DateTime dataFimAgendamento, DateTime periodoInativoInicio, DateTime periodoInativoFim)
        {
            if (periodoInativoInicio == default || periodoInativoFim == default)
                return false;

            var diaInicial = dataInicioAgendamento.Date.AddDays(-1);
            var ultimoDia = dataFimAgendamento.Date;

            for (var dia = diaInicial; dia <= ultimoDia; dia = dia.AddDays(1))
            {
                var intervaloInativo = MontarIntervalo(dia, periodoInativoInicio.TimeOfDay, periodoInativoFim.TimeOfDay);
                if (PossuiIntersecao(dataInicioAgendamento, dataFimAgendamento, intervaloInativo.Inicio, intervaloInativo.Fim))
                    return true;
            }

            return false;
        }

        private static (DateTime Inicio, DateTime Fim) MontarIntervalo(DateTime dataBase, TimeSpan horaInicio, TimeSpan horaFim)
        {
            var inicio = dataBase.Date.Add(horaInicio);
            var fim = horaInicio <= horaFim
                ? dataBase.Date.Add(horaFim)
                : dataBase.Date.AddDays(1).Add(horaFim);

            if (horaInicio == horaFim)
                fim = inicio.AddDays(1);

            return (inicio, fim);
        }

        private static bool PossuiIntersecao(DateTime inicioA, DateTime fimA, DateTime inicioB, DateTime fimB)
        {
            return inicioA < fimB && fimA > inicioB;
        }
    }
}
