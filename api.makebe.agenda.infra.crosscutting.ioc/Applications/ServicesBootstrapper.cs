using api.makebe.agenda.applications.Factorys;
using api.makebe.agenda.applications.Factorys.Interfaces;
using api.makebe.agenda.applications.Helpers;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Services.Agendamentos;
using api.makebe.agenda.applications.Services.Agendas;
using api.makebe.agenda.applications.Services.Colaboradores;
using api.makebe.agenda.applications.Services.Enderecos;
using api.makebe.agenda.applications.Services.Lojas;
using api.makebe.agenda.applications.Services.Portifolios;
using api.makebe.agenda.applications.Services.Servicos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Applications
{
    public static class ServicesBootstrapper
    {
        public static void InitializeServicesBootstrapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILojaApplicationService, LojaApplicationService>();
            services.AddScoped<ITipoLojaApplicationService, TipoLojaApplicationService>();
            services.AddScoped<IEnderecoApplicationService, EnderecoApplicationService>();
            services.AddScoped<IPortifolioApplicationService, PortifolioApplicationService>();
            services.AddScoped<IPortifolioImagemApplicationService, PortifolioImagensApplicationService>();
            services.AddScoped<IColaboradorApplicationService, ColaboradorApplicationService>();
            services.AddScoped<IServicoApplicationService, ServicosApplicationService>();
            services.AddScoped<IColaboradorProfissionalApplicationService, ColaboradorProfissionalApplicationService>();
            services.AddScoped<ITipoPortifolioApplicationService, TipoPortifolioApplicationService>();
            services.AddScoped<IEnderecoContextApplicationService, EnderecoLojaApplicationService>();
            services.AddScoped<IEnderecoContextApplicationService, EnderecoColaboradorApplicationService>();
            services.AddScoped<IPortifolioContextApplicationService, PortifolioColaboradorApplicationService>();
            services.AddScoped<IPortifolioContextApplicationService, PortifolioLojaApplicationService>();
            services.AddScoped<IEnderecoValidacaoApplicationService, EnderecoValidacaoApplicationService>();
            services.AddScoped<IAgendaApplicationService, AgendaApplicationService>();
            services.AddScoped<IAgendamentoApplicationService, AgendamentoApplicationService>();
            services.AddScoped<IAgendamentoColaboradorApplicationService, AgendamentoColaboradorApplicationService>();
            services.AddScoped(typeof(IContextFactory<>), typeof(ContextFactory<>));

            ConfigHelper.Initialize(configuration);

        }
    }
}
