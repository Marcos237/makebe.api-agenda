namespace ColaboradoresProfissionalEvent
{
    public interface IColaboradorProfissionalEvent
    {
        int LojaId { get; set; }
        IEnumerable<ColaboradorProfissionalEvent> ColaboradoresProfissionais { get; set; }
        DateTime DataEvento { get; set; }
    }
}
