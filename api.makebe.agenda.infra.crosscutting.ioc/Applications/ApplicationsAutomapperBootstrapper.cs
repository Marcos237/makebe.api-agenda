using api.makebe.agenda.applications.AutoMapper;
using api.makebe.agenda.applications.Mappers.EnderecoMappers;
using api.makebe.agenda.applications.Mappers.LojaMappers;
using api.makebe.agenda.applications.Mappers.LojaPortifolios;
using api.makebe.agenda.applications.Mappers.LojaPortifoliosMappers;
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
                mc.AddProfile(new PaginacaoLojaResponseMap());
                mc.AddProfile(new LojaPayloadMapper());
                mc.AddProfile(new LojaDTOResponseMapper());
                mc.AddProfile(new LojaResponseMapper());
                mc.AddProfile(new PaginacaoLojaPayloadMap());
                mc.AddProfile(new SessaoMapper());
                mc.AddProfile(new EnderecoDTOEnderecoMapper());
                mc.AddProfile(new LojaPortifolioImagemMap());
                mc.AddProfile(new LojaPortifolioMap());
                mc.AddProfile(new LojaPortifolioPayloadMap());
                mc.AddProfile(new LojaPortifolioImagensArquivoMap());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
