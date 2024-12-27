namespace api.makebe.agenda.applications.Models.Payloads
{
    public class ColaboradorProfissionalPayload
    {
        public int Id { get; set; }
        public int ColaboradorId { get; set; }
        public int LojaId { get; set; }
        public int ServicoId { get; set; }
        public string? Descricao { get; set; }
    }
}
