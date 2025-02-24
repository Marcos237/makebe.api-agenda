using api.makebe.agenda.domain.DTO;

namespace api.makebe.agenda.applications.Models.Payloads
{
    public class PortifolioPayload
    {
        public int Id { get; set; }

        public int LojaId { get; set; }
        public int LojaPortifolioId { get; set; }
        public int ColaboradorPortifolioId { get; set; }

        public int ColaboradorId { get; set; }

        public int TipoUsuarioId { get; set; }

        public string? Titulo { get; set; }

        public string? SubTitulo { get; set; }

        public string? Texto { get; set; }

        public IEnumerable<PortifolioImagemDTO>? PortifolioImagens { get; set; }
        public PortifolioPayload()
        {
            PortifolioImagens = new List<PortifolioImagemDTO>();  
        }
    }
}
