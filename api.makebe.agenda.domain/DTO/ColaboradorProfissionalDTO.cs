using api.makebe.agenda.domain.Entidades;

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
        public string? UrlImagem { get; set; }
        public bool Status { get; set; }
        public string? NomeColaboradorRazaoSocial
        {
            get => $"{NomeColaborador} - {RazaoSocial}";
        }
        public DateTime PeriodoInativoInicio { get; set; }
        public DateTime PeriodoInativoFim { get; set; }
        public string? PeriodoInativoInicioExtenso { get; set; }
        public string? PeriodoInativoFimExtenso { get; set; }
        public IEnumerable<PortifolioImagemDTO>? Imagens { get; set; }

        public IEnumerable<ColaboradorServicos>? Servicos { get; set; }
        public string? Texto { get; set; }
    }
}
