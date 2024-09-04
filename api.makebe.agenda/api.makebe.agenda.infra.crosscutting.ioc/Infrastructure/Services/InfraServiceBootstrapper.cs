using api.makebe.agenda.infra.crosscutting.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Infrastructure.Services
{
    public static class InfraServiceBootstrapper
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddTransient<ILogCrossCuttingService, LogCrossCuttingService>();
            services.AddTransient<IRecaptchaValidatorCrossCuttingService, RecaptchaValidatorCrossCuttingService>();  
        }
    }
}
