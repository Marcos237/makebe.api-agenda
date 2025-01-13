namespace api.makebe.agenda.domain.Entidades
{
    public class PortifolioImagens
    {
        public int Id { get; set; }

        public int PortifolioId { get; set; }

        public string? TituloImagem { get; set; }

        public bool Status { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAtualizacao { get; set; }
        public Arquivo? Imagem { get; set; }

    }
}
