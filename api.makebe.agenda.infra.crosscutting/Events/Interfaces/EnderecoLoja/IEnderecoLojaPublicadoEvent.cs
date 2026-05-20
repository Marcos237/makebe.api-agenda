namespace EnderecoLojaEvent
{
    public interface IEnderecoLojaPublicadoEvent
    {
        int Id { get; set; }
        IEnumerable<EnderecoLojaEvent> Lojas { get; set; }
        DateTime DataEvento { get; set; }
    }
}
