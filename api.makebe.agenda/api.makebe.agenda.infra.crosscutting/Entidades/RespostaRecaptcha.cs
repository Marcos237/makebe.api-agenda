namespace api.makebe.agenda.infra.crosscutting.Entidades
{
    public class RespostaRecaptcha
    {
        public bool Success { get; set; }
        public string[]? ErrorCodes { get; set; }
    }
}
