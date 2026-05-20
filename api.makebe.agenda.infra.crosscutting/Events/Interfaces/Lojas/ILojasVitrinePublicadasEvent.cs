namespace LojasEvent
{
    public interface ILojasVitrinePublicadasEvent
    {
        string Tipo { get; set; }
        IEnumerable<LojaVitrineEvent> Lojas { get; set; }
        DateTime DataEvento { get; set; }
    }
}
