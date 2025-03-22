using api.makebe.agenda.applications.AutoMapper;
using api.makebe.agenda.applications.Mappers.ColaboradorMappers;
using api.makebe.agenda.applications.Mappers.ColaboradorProfissionalMapper;
using api.makebe.agenda.applications.Mappers.EnderecoMappers;
using api.makebe.agenda.applications.Mappers.LojaMappers;
using api.makebe.agenda.applications.Mappers.LojaPortifolios;
using api.makebe.agenda.applications.Mappers.LojaPortifoliosMappers;
using api.makebe.agenda.applications.Mappers.PortifoliosMappers;
using api.makebe.agenda.applications.Mappers.ServicoMappers;
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
                mc.AddProfile(new EnderecoPayloadToEnderecoMap());
                mc.AddProfile(new PortifolioImagemMap());
                mc.AddProfile(new PortifolioMap());
                mc.AddProfile(new PortifolioPayloadMap());
                mc.AddProfile(new PortifolioImagensArquivoMap());
                mc.AddProfile(new ColaboradorPayloadToUsuarioConsultadoEventMap());
                mc.AddProfile(new ColaboradorPayloadToUsuarioRegistradoEventMap());
                mc.AddProfile(new ColaboradorPayloadToColaboradorMap());
                mc.AddProfile(new UsuarioEventToUsuarioDTOMap());
                mc.AddProfile(new ColaboradorPayloadToLojaColaboradorMap());
                mc.AddProfile(new UsuarioPaginadoDTOToUsuarioPaginadoEvent());
                mc.AddProfile(new UsuarioDTOToColaboradorDTOMap());
                mc.AddProfile(new ColaboradorProfissionalPayloadToColaboradoProfissionalMap());
                mc.AddProfile(new ColaboradorProfissionalToColaboradoProfissionalDTOMap());
                mc.AddProfile(new PortifolioPayloadToColaboradorPortifolioMap());
                mc.AddProfile(new PortifolioPayloadToLojaPortifolioMap());
                mc.AddProfile(new EnderecoPayloadToLojaEnderecoMap());
                mc.AddProfile(new EnderecoPayloadToColaboradorEnderecoMap());
                mc.AddProfile(new ServicoServicoDTOMap());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
