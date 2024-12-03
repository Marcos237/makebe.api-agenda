namespace api.makebe.agenda.infra.crosscutting.Entidades.Constants
{
    public static class RabbitMQConstant
    {
        public const string HostName = "rabbitMqHost";
        public const string User = "rabbitMqUser";
        public const string Senha = "rabbitMqPass";
        public const string UsuarioQueue = "usuario-queue";
        public const string FilaVazia = "filaVazia";
        public const string ErroFila = "Fila inválida";
        public const string NomeFilaPaginacaoUsuario = "usuario-paginado";
        public const string NomeExchange = "makebe-exchange";
    }
}
