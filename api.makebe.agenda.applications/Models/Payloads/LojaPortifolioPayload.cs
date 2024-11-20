using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Models.Payloads
{
    public class LojaPortifolioPayload
    {
        public int Id { get; set; }

        public int LojaId { get; set; }

        public string? Titulo { get; set; }

        public string? SubTitulo { get; set; }

        public string? Texto { get; set; }

        public IEnumerable<LojaPortifolioImagemDTO>? LojaPortifolioImagens { get; set; }
        public LojaPortifolioPayload()
        {
            LojaPortifolioImagens = new List<LojaPortifolioImagemDTO>();  
        }
    }
}
