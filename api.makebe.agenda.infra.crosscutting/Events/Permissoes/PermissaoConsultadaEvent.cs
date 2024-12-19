using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Interfaces;

namespace api.makebesession.infra.crosscutting.Events.Permissoes
{
    public class PermissoesConsultadasEvent : IPermissaoConsultadaEvent
    {
        public List<PermissaoEvent> Permissoes { get; set; }
        public PermissoesConsultadasEvent()
        {
            Permissoes = new List<PermissaoEvent>();
        }
    }
}
