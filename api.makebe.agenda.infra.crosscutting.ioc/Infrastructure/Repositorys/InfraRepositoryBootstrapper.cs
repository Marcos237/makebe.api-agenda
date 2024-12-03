using api.makebe.agenda.infra.crosscutting.Repositorys;
using api.makebe.agenda.infra.crosscutting.Repositorys.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Infrastructure.Repositorys
{
    public static class InfraRepositoryBootstrapper
    {
        public static void InitializeInfraRepositoryBootstrapper(this IServiceCollection services)
        {
            services.AddScoped<ILogRepository, LogRepository>(); 
        }
    }
}
