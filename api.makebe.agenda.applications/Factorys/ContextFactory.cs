using api.makebe.agenda.applications.Factorys.Interfaces;
using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.Enums;

namespace api.makebe.agenda.applications.Factorys
{
    public class ContextFactory<T> : IContextFactory<T> where T : class
    {
        private readonly IEnumerable<T> _services;

        public ContextFactory(IEnumerable<T> services)
        {
            _services = services;
        }
        public async Task<T> ExecutarService(int tipo)
        {
            string termoBusca = string.Empty;

            if (tipo == (int)TipoUsuario.Loja)
                termoBusca = BaseConstant.InstanciaLoja;
            else
                termoBusca = BaseConstant.InstanciaColaborador;

            var instancia = _services.FirstOrDefault(service =>
                service.GetType().Name.IndexOf(termoBusca, StringComparison.OrdinalIgnoreCase) >= 0
            );

            return await Task.FromResult(instancia);
        }

    }
}
