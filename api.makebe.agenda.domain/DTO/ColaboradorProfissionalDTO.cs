namespace api.makebe.agenda.domain.DTO
{
    public class ColaboradorProfissionalDTO
    {
        public int Id { get; set; }
        public int ColaboradorId { get; set; }
        public string? UsuarioId { get; set; }
        public int LojaId { get; set; }
        public int ServicoId { get; set; }
        public string? Descricao { get; set; }
        public string? NomeColaborador { get; set; }
        public string? RazaoSocial { get; set; }
        public string? DescricaoServico { get; set; }
        public bool Status { get; set; }
    }
}
