using api.makebe.agenda.applications.AutoMapper;
using api.makebe.agenda.applications.Mappers.ColaboradorMappers;
using api.makebe.agenda.applications.Mappers.ColaboradorProfissionalMapper;
using api.makebe.agenda.applications.Mappers.EnderecoMappers;
using api.makebe.agenda.applications.Mappers.LojaMappers;
using api.makebe.agenda.applications.Mappers.LojaPortifolios;
using api.makebe.agenda.applications.Mappers.LojaPortifoliosMappers;
using AutoMapper;
using lib.makebe.Applications.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Applications
{
    public static class ApplicationsAutomapperBootstrapper 
    {
        public static void InitializeApplicationsAutomapperBootstrapper(this IServiceCollection services)
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
                mc.AddProfile(new ColaboradorPayloadToUsuarioConsultadoEventMap());
                mc.AddProfile(new ColaboradorPayloadToUsuarioRegistradoEventMap());
                mc.AddProfile(new ColaboradorPayloadToColaboradorMap());
                mc.AddProfile(new UsuarioEventToUsuarioDTOMap());
                mc.AddProfile(new ColaboradorPayloadToLojaColaboradorMap());
                mc.AddProfile(new UsuarioPaginadoDTOToUsuarioPaginadoEvent());
                mc.AddProfile(new UsuarioDTOToColaboradorDTOMap());
                mc.AddProfile(new ColaboradorProfissionalPayloadToColaboradoProfissionalMap());
                mc.AddProfile(new ColaboradorProfissionalToColaboradoProfissionalDTOMap());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
