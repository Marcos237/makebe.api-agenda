namespace api.makebe.agenda.domain.Constants
{
    public static class ColaboradorProfissionalConstant
    {
        public const string ColaboradorIdValidacao = "O colaborador não pode estar vazio.";
        public const string LojaIdValidacao = "O campo loja não pode estar vazio.";
        public const string ServicoValidacao = "O campo serviço não pode estar vazio.";
        public const string ServicoQuantidadeValidacao = "O colaborador pode possuir no máximo 10 serviços.";
        public const string ServicoDuplicadoValidacao = "Não é permitido adicionar o mesmo serviço mais de uma vez.";
        public const string PeriodoInativoInicioFimValidacao = "A data de inicio do periodo inativo deve ser menor que a data fim.";
    }
}
