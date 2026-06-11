using MassTransit;

namespace DesativarAgendamentoEvent
{
    [EntityName("desativar-agendamento")]
    public class DesativarAgendamentoMessage : IDesativarAgendamentoMessage
    {
        public int Id { get; set; }
    }
}
