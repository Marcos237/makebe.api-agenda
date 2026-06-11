namespace api.makebe.agenda.domain.Extensions
{
    public static class PaginacaoExtension
    {
        public static int CalcularTotalPaginas(this int total, int quantidadePagina)
        {
            if (quantidadePagina <= 0)
                return 0;

            return (total + quantidadePagina - 1) / quantidadePagina;
        }
    }
}
