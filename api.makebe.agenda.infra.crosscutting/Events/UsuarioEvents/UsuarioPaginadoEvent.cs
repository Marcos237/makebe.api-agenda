using api.makebe.agenda.infra.crosscutting.Entidades;
using MassTransit;

namespace api.makebe.agenda.infra.crosscutting.Events.UsuarioEvents
{
    public class UsuarioPaginadoEvent : IConsumer<PaginacaoEvent<UsuarioEvent>>
    {
        public Task Consume(ConsumeContext<PaginacaoEvent<UsuarioEvent>> context)
        {
            throw new NotImplementedException();
        }
    }
}
