using api.makebe.agenda.infra.data.interfaces;
using api.makebe.agenda.infra.data.Repositorys;
using api.makebe.agenda.infra.data.Uow.interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Data
{
    public class DataBootstrapper
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddScoped<DbAgenda>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
