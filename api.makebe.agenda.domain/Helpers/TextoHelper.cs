using api.makebe.agenda.domain.Constants;
using System.Globalization;
using System.Text.RegularExpressions;

namespace api.makebe.agenda.domain.Helpers
{
    public static class TextoHelper
    {
        public static string RemoverAcentos(string texto)
        {
            if (texto == null) return string.Empty;

            for (var i = 0; i < TextoConstant.comAcentos.Length; i++)
                texto = texto.Replace(TextoConstant.comAcentos[i].ToString(), TextoConstant.semAcentos[i].ToString());

            return texto;
        }

        public static string FormatarTextoParaUrl(string texto)
        {
            texto = RemoverAcentos(texto);

            var textoretorno = texto.Replace(" ", "");



            for (var i = 0; i < texto.Length; i++)
                if (!TextoConstant.permitidos.Contains(texto.Substring(i, 1))) { textoretorno = textoretorno.Replace(texto.Substring(i, 1), ""); }

            return textoretorno;
        }

        public static string GetNumeros(string texto)
        {
            var dados = string.IsNullOrEmpty(texto) ? "" : new String(texto.Where(Char.IsDigit).ToArray());
            return dados;
        }

        public static string AjustarTexto(string valor, int tamanho)
        {
            if (valor.Length > tamanho)
            {
                valor = valor.Substring(1, tamanho);
            }
            return valor;
        }

        public static string ToTitleCase(string texto)
        {
            return ToTitleCase(texto, false);
        }

        public static string ToTitleCase(string texto, bool manterOqueJaEstiverMaiusculo)
        {
            texto = texto.Trim();

            if (!manterOqueJaEstiverMaiusculo)
                texto = texto.ToLower();

            var textInfo = new CultureInfo("pt-BR", false).TextInfo;
            return textInfo.ToTitleCase(texto);
        }
        public static bool VerifyNumbers(string texto)
        {
            return Regex.IsMatch(texto, @"^\d+$");
        }
    }
}
