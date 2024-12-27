using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.crosscutting.Notifications;
using api.makebe.agenda.infra.crosscutting.Services;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Infrastructure.Services
{
    public static class InfraServiceCrossCuttingBootstrapper
    {
        public static void InitializeInfraServiceCrossCuttingBootstrapper(this IServiceCollection services)
        {
            services.AddScoped<ILogCrossCuttingService, LogCrossCuttingService>();
            services.AddScoped<IRecaptchaValidatorCrossCuttingService, RecaptchaValidatorCrossCuttingService>();
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddScoped<IContaEventCrossCuttingService, ContaEventCrossCuttingService>();
            services.AddScoped<IPermissaoEventCrossCuttingService, PermissaoEventCrossCuttingService>();
            services.AddScoped<IUsuarioEventCrossCuttingService, UsuarioEventCrossCuttingService>();
        }
    }
}
