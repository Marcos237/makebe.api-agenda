namespace ColaboradoresProfissionalEvent
{
    public interface IColaboradorPortifolioImagemPublicadoEvent
    {
        int Id { get; set; }
        IEnumerable<ColaboradorPortifolioImagemEvent> Imagens { get; set; }
        DateTime DataEvento { get; set; }
    }
}
