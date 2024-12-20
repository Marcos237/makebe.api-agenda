using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using api.makebesession.infra.crosscutting.Entidades;
using api.makebesession.infra.crosscutting.Events.Contas;

namespace api.makebe.agenda.applications.Helpers
{
    public static class ContaHelper
    {
        public static async Task<ContaEvent> BuscarContaPorUsuarioId(string usuarioId, IBusEvent busEvent)
        {
            var contaEvent = new ContaConsultadoPorIdEvent() { Id = PropiedadesHelper.ParseGuidOrDefault(usuarioId) };
            var conta = await busEvent.RequestAsync<ContaConsultadoPorIdEvent, ContaConsultadoPorIdEvent>(contaEvent, TimeSpan.FromSeconds(15));
            return conta?.ContaEvent ?? new ContaEvent();
        }
    }
}
