using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Applications
{
    public static class ServicesBootstrapper
    {
        public static void InitializeServicesBootstrapper(this IServiceCollection services)
        {
            services.AddScoped<ILojaApplicationService, LojaApplicationService>();
            services.AddScoped<ITipoLojaApplicationService, TipoLojaApplicationService>();
            services.AddScoped<IEnderecoApplicationService, EnderecoApplicationService>();
            services.AddScoped<ILojaEnderecoApplicationService, LojaEnderecoApplicationService>();
            services.AddScoped<ILojaPortifolioApplicationService, LojaPortifolioApplicationService>();
            services.AddScoped<ILojaPortifolioImagemApplicationService, LojaPortifolioImagensApplicationService>();
            services.AddScoped<IColaboradorApplicationService, ColaboradorApplicationService>();
            services.AddScoped<IServicoApplicationService, ServicosApplicationService>();
            services.AddScoped<IColaboradorProfissionalApplicationService, ColaboradorProfissionalApplicationService>();
        }
    }
}
