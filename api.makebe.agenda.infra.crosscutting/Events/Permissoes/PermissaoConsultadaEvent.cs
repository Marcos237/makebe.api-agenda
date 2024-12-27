using api.makebe.agenda.infra.crosscutting.Entidades;

namespace PermissoesEvent
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
