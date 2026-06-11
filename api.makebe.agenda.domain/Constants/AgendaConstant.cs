namespace api.makebe.agenda.domain.Constants
{
    public static class AgendaConstant
    {
        public const string DataAberturaInvalido = "A data de abertura da agenda não pode estar nula.";
        public const string DataFechamentoInvalido = "A data de fechamento da agenda não pode estar nula.";
        public const string DataAbrturaFechamentoInvalido = "A data de abertura não pode ser menor ou igual que a data de fechemento.";
        public const string DataBloqueioFechamentoInvalido = "A data de bloqueio início não pode ser menor ou igual que a data de  bloqueio fim.";
        public const string AgendaAbertaInicioTipo = "AgendaAberta";
        public const string AgendaBloqueadaInicioTipo = "AgendaBloqueadaAberta";
        public const string DiaSemanaInvalido = "O dia da semana de ínicio não pode ser menor que o dia da semana final.";
        public const string DiaSemanaInicioInvalido = "O dia  de início da semana não pode ser nulo";
        public const string DiaSemanaFimInvalido = "O dia de Fim da semana não pode ser nulo";
        public const string LojaInvalido = "O campo loja não pode ser nulo.";
        public const string ColaboradorInvalido = "O campo Colaborador não pode ser nulo."; 
    }
}
