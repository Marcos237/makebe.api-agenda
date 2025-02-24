using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Services;
using api.makebe.agenda.applications.Strategys.Interfaces.Enderecos;
using api.makebe.agenda.applications.Strategys.Interfaces.Portifolios;
using api.makebe.agenda.applications.Strategys.Services.Enderecos;
using api.makebe.agenda.applications.Strategys.Services.Portifolios;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Applications
{
    public static class ServicesBootstrapper
    {
        public static void InitializeServicesBootstrapper(this IServiceCollection services)
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
            services.AddScoped<IPortifolioPersisteStrategy<PortifolioPayload>, ColaboradorPortifolioPersiteStrategy>();
            services.AddScoped<IPortifolioPersisteStrategy<PortifolioPayload>, LojaPortifolioPersisteStrategy>();
            services.AddScoped<IPortifolioPersisteStrategyContext<PortifolioPayload>, PortifolioPersisteStrategyContext<PortifolioPayload>>();

            services.AddScoped<IPortifolioBuscaStrategyContext, PortifolioBuscaStrategyContext>();
            services.AddScoped<IPortifolioBuscaStrategy, ColaboradorPortifolioBuscaStrategy>();
            services.AddScoped<IPortifolioBuscaStrategy, LojaPortifolioBuscaStrategy>();

            services.AddScoped<IEnderecoBuscaStrategyContext, EnderecoBuscaStrategyContext>();
            services.AddScoped<IEnderecoBuscaStrategy, LojaEnderecoBuscaStrategy>();
            services.AddScoped<IEnderecoBuscaStrategy, ColaboradorEnderecoBuscaStrategy>();

            services.AddScoped<IEnderecoPersisteStrategyContext<EnderecoPayload>, EnderecoPersisteStrategyContext<EnderecoPayload>>();
            services.AddScoped<IEnderecoPersisteStrategy<EnderecoPayload>, LojaEnderecoPersisteStrategy>();
            services.AddScoped<IEnderecoPersisteStrategy<EnderecoPayload>, ColaboradorEnderecoPersisteStrategy>();

        }
    }
}
