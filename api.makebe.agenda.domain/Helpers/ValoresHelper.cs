using System.Globalization;

namespace api.makebe.agenda.domain.Helpers
{
    public static class ValoresHelper
    {
        public static decimal GetValorSemTexto(string texto)
        {
            var dados = string.IsNullOrEmpty(texto) ? "" : new String(texto.Where(Char.IsDigit).ToArray());
            return Convert.ToDecimal(dados);
        }
        public static string SetValorExtenso(decimal valor)
        {
            var dados = $"{valor.ToString("C")}";
            return dados;
        }
        public static string SetPeridoExtenso(decimal valor)
        {
            if (valor > 0)
            {
                var dadosSplit = valor.ToString()?.Split(',') ?? [];
                if (dadosSplit.Length > 1)
                {
                    var valorFormatado = $"0{dadosSplit[0].ToString()}:{dadosSplit[1].ToString()}";
                    return valorFormatado;
                }
                return $"{valor} : 00";

            }
            var dados = $"00:00";
            return dados;
        }

        public static DateTime? SetDateTimeCustomer(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return null;

            if (DateTime.TryParseExact(
                    valor,
                    "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime dataConvertida))
            {
                return dataConvertida;
            }

            return null;
        }

        public static DateTime? SetDateHourMinuteCustomer(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return null;

            if (DateTime.TryParseExact(
                    valor,
                    "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime dataConvertida))
            {
                return new DateTime(1, 1, 1, dataConvertida.Hour, dataConvertida.Minute, 0);
            }

            return null;
        }
        public static DateTime? MontarDate(string? valor, string? data)
        {
            if (string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(valor))
                return null;

            if (!DateTime.TryParse(data.Trim(), out var baseDate))
                return null;

            if (TimeSpan.TryParse(valor.Trim(), out var hora))
                return baseDate.Date.Add(hora);

            return null;
        }

        public static DateTime? ConverterParaData(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return null;

            return DateTime.TryParse(valor, out var data)
                ? data
                : null;
        }
    }
}
