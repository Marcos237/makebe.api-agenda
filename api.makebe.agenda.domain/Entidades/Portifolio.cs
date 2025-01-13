namespace api.makebe.agenda.domain.Entidades
{
    public class Portifolio
    {
        public int Id { get; set; }

        public string? RazaoSocial { get; set; }

        public string? Titulo { get; set; }

        public string? SubTitulo { get; set; }

        public string? Texto { get; set; }

        public bool Status { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public IEnumerable<PortifolioImagens>? LojaPortifolioImagens { get; set; }
    }
}
