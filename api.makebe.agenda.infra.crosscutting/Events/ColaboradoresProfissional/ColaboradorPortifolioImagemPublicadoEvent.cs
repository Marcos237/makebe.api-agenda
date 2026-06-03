using MassTransit;

namespace ColaboradoresProfissionalEvent
{
    [EntityName("colaborador-portifolio-imagem-publicados")]
    public class ColaboradorPortifolioImagemPublicadoEvent : IColaboradorPortifolioImagemPublicadoEvent
    {
        public int Id { get; set; }
        public IEnumerable<ColaboradorPortifolioImagemEvent> Imagens { get; set; } = Enumerable.Empty<ColaboradorPortifolioImagemEvent>();
        public DateTime DataEvento { get; set; } = DateTime.Now;
    }
}
