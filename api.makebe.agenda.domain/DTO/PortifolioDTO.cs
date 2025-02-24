namespace api.makebe.agenda.domain.DTO
{
    public class PortifolioDTO
    {
        public int Id { get; set; }

        public string? ContaId { get; set; }
        public string? UsuarioId { get; set; }
        public int TipoUsuarioId { get; set; }
        public string? Titulo { get; set; }
        public string? SubTitulo { get; set; }
        public string? Texto { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
        public int ColaboradorPortifolioId { get; set; }
        public string? NomeColaborador { get; set; }
        public int ColaboradorId { get; set; }
        public int LojaId { get; set; }
        public int LojaPortifolioId { get; set; }
        public string? RazaoSocial { get; set; }
        public IEnumerable<PortifolioImagemDTO>? PortifolioImagens { get; set; }
    }
}
