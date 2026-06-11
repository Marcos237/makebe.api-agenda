namespace api.makebe.agenda.domain.Entidades
{
    public class EmailEnvio
    {
        public int Id { get; set; }
        public string? DadosModelo { get; set; }
        public string? DadosEnvio { get; set; }
        public string? Pasta { get; set; }
        public string? NomeArquivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Processado { get; set; }
        public int Tentativas { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public string? Erro { get; set; }
    }
}
