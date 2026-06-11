using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Extensions;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class PeriodoDisponivelAgendamentoDomainService : IPeriodoDisponivelAgendamentoDomainService
    {
        public async Task<IEnumerable<PeriodoDTO>> MontarPeriodosDisponiveis(DateTime data, decimal periodoServico, IEnumerable<AgendamentoColaboradorPeriodoDTO> agendas)
        {
            if (periodoServico <= 0)
                return Enumerable.Empty<PeriodoDTO>();

            var agendasLista = agendas?.ToList() ?? new List<AgendamentoColaboradorPeriodoDTO>();
            var inicioDia = data.Date;
            var fimDiaExclusivo = inicioDia.AddDays(1);
            var duracaoSlot = periodoServico.ParaTimeSpan();

            var indisponiveis = new List<(DateTime Inicio, DateTime Fim)>();

            foreach (var agenda in agendasLista)
            {
                indisponiveis.AddRange(MontarIntervalosDiarios(inicioDia, agenda.PeriodoInativoInicio, agenda.PeriodoInativoFim));
                indisponiveis.AddRange(MontarIntervalosDiarios(inicioDia, agenda.AgendaBloqueadaInicio, agenda.AgendaBloqueadaFim));
            }

            var indisponiveisMesclados = MesclarIntervalos(indisponiveis, inicioDia, fimDiaExclusivo);
            var disponiveis = MontarIntervalosDisponiveis(inicioDia, fimDiaExclusivo, indisponiveisMesclados);

            var periodos = ReagruparPeriodosMarcar(disponiveis, agendasLista, duracaoSlot);
            var retorno = periodos
                .GroupBy(periodo => new { periodo.Inicio, periodo.Fim })
                .Select(grupo => grupo.OrderByDescending(periodo => periodo.IsAgendado).First())
                .OrderBy(periodo => periodo.Inicio)
                .ToList();

            return await Task.FromResult(retorno);
        }


        private static IEnumerable<(DateTime Inicio, DateTime Fim)> MontarIntervalosDiarios(DateTime dataBase, DateTime inicio, DateTime fim)
        {
            if (inicio == default || fim == default)
                return Enumerable.Empty<(DateTime Inicio, DateTime Fim)>();

            var horaInicio = inicio.TimeOfDay;
            var horaFim = fim.TimeOfDay;
            var inicioDia = dataBase.Date;
            var fimDiaExclusivo = inicioDia.AddDays(1);

            if (horaInicio == horaFim)
                return new[] { (inicioDia, fimDiaExclusivo) };

            if (horaInicio < horaFim)
            {
                return new[]
                {
                    (inicioDia.Add(horaInicio), inicioDia.Add(horaFim))
                };
            }

            return new[]
            {
                (inicioDia, inicioDia.Add(horaFim)),
                (inicioDia.Add(horaInicio), fimDiaExclusivo)
            };
        }

        private static List<(DateTime Inicio, DateTime Fim)> MesclarIntervalos(IEnumerable<(DateTime Inicio, DateTime Fim)> intervalos, DateTime inicioLimite, DateTime fimLimite)
        {
            var ordenados = intervalos
                .Select(intervalo => (
                    Inicio: intervalo.Inicio < inicioLimite ? inicioLimite : intervalo.Inicio,
                    Fim: intervalo.Fim > fimLimite ? fimLimite : intervalo.Fim))
                .Where(intervalo => intervalo.Inicio < intervalo.Fim)
                .OrderBy(intervalo => intervalo.Inicio)
                .ToList();

            var retorno = new List<(DateTime Inicio, DateTime Fim)>();
            foreach (var intervalo in ordenados)
            {
                if (!retorno.Any())
                {
                    retorno.Add(intervalo);
                    continue;
                }

                var ultimo = retorno[^1];
                if (intervalo.Inicio <= ultimo.Fim)
                {
                    retorno[^1] = (ultimo.Inicio, intervalo.Fim > ultimo.Fim ? intervalo.Fim : ultimo.Fim);
                    continue;
                }

                retorno.Add(intervalo);
            }

            return retorno;
        }

        private static List<(DateTime Inicio, DateTime Fim)> MontarIntervalosDisponiveis(DateTime inicioDia, DateTime fimDiaExclusivo, IEnumerable<(DateTime Inicio, DateTime Fim)> indisponiveis)
        {
            var retorno = new List<(DateTime Inicio, DateTime Fim)>();
            var cursor = inicioDia;

            foreach (var intervalo in indisponiveis)
            {
                if (cursor < intervalo.Inicio)
                    retorno.Add((cursor, intervalo.Inicio));

                if (intervalo.Fim > cursor)
                    cursor = intervalo.Fim;
            }

            if (cursor < fimDiaExclusivo)
                retorno.Add((cursor, fimDiaExclusivo));

            return retorno;
        }
        public List<PeriodoDTO> ReagruparPeriodosMarcar(
            List<(DateTime Inicio, DateTime Fim)> disponiveis,
            List<AgendamentoColaboradorPeriodoDTO>? agendas,
            TimeSpan duracaoSlot)
        {
            var periodos = new List<PeriodoDTO>();

            DateTime? proximoInicio = null;

            foreach (var intervalo in disponiveis.OrderBy(x => x.Inicio))
            {
                var cursor = proximoInicio ?? intervalo.Inicio;

                while (cursor.Add(duracaoSlot) <= intervalo.Fim)
                {
                    var agenda = agendas?
                        .FirstOrDefault(a =>
                            cursor >= a.DataInicioAgendamento &&
                            cursor < a.DataTerminoAgendamento);

                    if (agenda != null)
                    {
                        var duracaoAgendada = agenda.Periodo?.ParaTimeSpan() ?? TimeSpan.Zero;
                        var fimAgendamento = cursor.Add(duracaoAgendada);

                        periodos.Add(new PeriodoDTO
                        {
                            Inicio = cursor,
                            Fim = fimAgendamento,
                            IsAgendado = true
                        });

                        cursor = fimAgendamento;
                    }
                    else
                    {
                        var fim = cursor.Add(duracaoSlot);

                        if (fim > intervalo.Fim)
                            break;

                        periodos.Add(new PeriodoDTO
                        {
                            Inicio = cursor,
                            Fim = fim,
                            IsAgendado = false
                        });

                        cursor = fim;
                    }
                }

                proximoInicio = null;
            }

            return periodos;
        }
    }
}
