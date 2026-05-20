namespace ColaboradoresProfissionalEvent
{
    public class ColaboradorProfissionalEvent
    {
        public int Id { get; set; }

        public int ColaboradorId { get; set; }

        public string? UsuarioId { get; set; }
        public string? Nome { get; set; }

        public int LojaId { get; set; }

        public int ServicoId { get; set; }

        public string? DescricaoServico { get; set; }
        public string? UrlImagem { get; set; }
    }
}
