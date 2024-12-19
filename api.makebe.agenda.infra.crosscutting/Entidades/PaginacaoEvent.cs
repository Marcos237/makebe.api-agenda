namespace api.makebe.agenda.infra.crosscutting.Entidades
{
    public class PaginacaoEvent<T> where T : class
    {
        public int quantidadePagina { get; set; } = 10;
        public int totalPaginas { get; set; } = 1;
        public int total { get; set; } = 0;
        public int paginaAtual { get; set; } = 1;
        public int registroInicial { get; set; } = 1;
        public IEnumerable<T>? objetos { get; set; }
        public T? objetoPesquisa { get; set; }
        public IEnumerable<string>? idsPesquisa { get; set; }
    }
}
