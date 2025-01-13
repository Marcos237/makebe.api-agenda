namespace api.makebe.agenda.domain.DTO
{
    public class TipoPortifolioDTO
    {
        public int Id { get; set; }
        public int TipoPortifolioUsuarioId { get; set; }
        public string? Descricao { get; set; }
        public string? Label { get; set; }
        public string? NomeTipo { get; set; }
        public string? Titulo { get; set; }
        public bool Status { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
