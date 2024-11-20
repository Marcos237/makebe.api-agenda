namespace api.makebe.agenda.domain.DTO
{
    public class LojaPortifolioImagemDTO
    {
        public int LojaPortifolioImagemId { get; set; }
        public int LojaPortifolioId { get; set; }
        public string? UrlImagem { get; set; }
        public string? NomeImagem { get; set; }
        public string? TituloImagem { get; set; }
    }
}
