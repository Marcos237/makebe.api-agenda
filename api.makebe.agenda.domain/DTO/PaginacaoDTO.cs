namespace api.makebe.agenda.domain.DTO
{
    public class PaginacaoDTO<T> where T : class
    {
        public int quantidadePagina { get; set; } = 6;
        public int totalPaginas { get; set; } = 1;
        public int total { get; set; } = 0;
        public int paginaAtual { get; set; } = 1;
        public int registroInicial { get; set; } = 1;
        public IEnumerable<T>? objetos { get; set; }
        public T? objetoPesquisa { get; set; }
    }
}
