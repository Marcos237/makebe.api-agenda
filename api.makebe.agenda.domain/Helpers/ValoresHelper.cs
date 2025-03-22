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
            if(valor > 0)
            {
                var dadosSplit = valor.ToString()?.Split(',') ?? [];
                if(dadosSplit.Length > 1)
                {
                    var valorFormatado = $"0{dadosSplit[0].ToString()}:{dadosSplit[1].ToString()}";
                    return valorFormatado;
                }
                return $"{valor} : 00";

            }
            var dados = $"00:00";
            return dados;
        }
    }
}
