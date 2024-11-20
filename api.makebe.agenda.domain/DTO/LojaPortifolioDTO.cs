using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.DTO
{
    public class LojaPortifolioDTO
    {
        public int Id { get; set; }

        public int LojaId { get; set; }
        public string? RazaoSocial { get; set; }

        public string? Titulo { get; set; }

        public string? SubTitulo { get; set; }

        public string? Texto { get; set; }

        public bool Status { get; set; }

        public DateTime DataCadastro { get; set; }

        public IEnumerable<LojaPortifolioImagemDTO>? LojaPortifolioImagens { get; set; }
    }
}
