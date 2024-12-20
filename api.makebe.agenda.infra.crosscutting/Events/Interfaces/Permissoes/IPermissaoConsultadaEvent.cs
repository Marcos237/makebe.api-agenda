using api.makebesession.infra.crosscutting.Entidades;

namespace api.makebesession.infra.crosscutting.Events.Interfaces
{
    public interface IPermissaoConsultadaEvent
    {
        public List<PermissaoEvent> Permissoes { get; set; }

    }
}
