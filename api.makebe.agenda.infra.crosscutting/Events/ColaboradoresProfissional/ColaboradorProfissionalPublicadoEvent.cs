using MassTransit;

namespace ColaboradoresProfissionalEvent
{
    [EntityName("colaborador-profissional-vitrine-publicados")]
    public class ColaboradorProfissionalPublicadoEvent : IColaboradorProfissionalEvent
    {
        public int LojaId { get; set; }
        public IEnumerable<ColaboradorProfissionalEvent> ColaboradoresProfissionais { get; set; } = Enumerable.Empty<ColaboradorProfissionalEvent>();
        public DateTime DataEvento { get; set; } = DateTime.Now;
    }
}
