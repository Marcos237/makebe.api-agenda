using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Applications
{
    public class ServicesBootstrapper
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddTransient<ILojaApplicationService, LojaApplicationService>();
            services.AddTransient<ITipoLojaApplicationService, TipoLojaApplicationService>();
            services.AddTransient<IEnderecoApplicationService, EnderecoApplicationService>();
        }
    }
}
