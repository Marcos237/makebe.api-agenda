using api.makebe.agenda.infra.crosscutting.Entidades;

namespace PermissoesEvent
{
    public interface IPermissaoConsultadaEvent
    {
        public List<PermissaoEvent> Permissoes { get; set; }

    }
}
