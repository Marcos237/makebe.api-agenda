using api.makebe.agenda.applications.AutoMapper;
using api.makebe.agenda.applications.Mappers.LojaMappers;
using AutoMapper;
using lib.makebe.Applications.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Applications
{
    public class ApplicationsAutomapperBootstrapper 
    {
        public static void Initialize(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UsuarioSessaoMapper());
                mc.AddProfile(new LojaPayloadMapper());
                mc.AddProfile(new LojaResponseMapper());
                mc.AddProfile(new LojaResponseMapper());
                mc.AddProfile(new SessaoMapper());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
