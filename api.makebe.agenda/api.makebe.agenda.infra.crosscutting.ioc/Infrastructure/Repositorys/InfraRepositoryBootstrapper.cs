using api.makebe.agenda.infra.crosscutting.Repositorys;
using api.makebe.agenda.infra.crosscutting.Repositorys.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Infrastructure.Repositorys
{
    public class InfraRepositoryBootstrapper
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddTransient<ILogRepository, LogRepository>();
        }
    }
}
