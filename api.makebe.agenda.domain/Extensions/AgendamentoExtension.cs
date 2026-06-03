using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Helpers;
using System;
using System.Globalization;

namespace api.makebe.agenda.domain.Extensions
{
    public static class AgendamentoExtension
    {
        public static DateTime SomarPeriodo(this decimal periodo, DateTime dataEntrada)
        {
            var valor = periodo.ToString("0.00").Replace(",", ".");
            var partes = valor.Split('.');

            int horas = Convert.ToInt32(partes[0]);
            int minutos = Convert.ToInt32(partes[1]);

            return dataEntrada
                .AddHours(horas)
                .AddMinutes(minutos);
        }

        public static DateTime MontarDataTermino(this AgendamentoDTO? agendamento)
        {
            var dataInicio = ValoresHelper.MontarDate(agendamento?.DataInicioAgendamentoExtenso, agendamento?.Data) ?? DateTime.Now;
            return (agendamento?.Periodo.SomarPeriodo(dataInicio) ?? dataInicio).AddMinutes(-1);
        }

        public static TimeSpan ParaTimeSpan(this decimal valor)
        {
            var texto = valor.ToString("0.##", CultureInfo.InvariantCulture);

            var partes = texto.Split('.');

            var horas = int.Parse(partes[0]);
            var minutos = partes.Length > 1
                ? int.Parse(partes[1].PadRight(2, '0'))
                : 0;

            return new TimeSpan(horas, minutos, 0);
        }
    }
}
