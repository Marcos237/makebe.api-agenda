using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.infra.data.Repositorys;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Domains
{
    public static class RepositoryBootstrapper
    {
        public static void InitializeRepositoryBootstrapper(this IServiceCollection services)
        {
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<ILojaRepository, LojaRepository>();
            services.AddScoped<IContaLojaRepository, ContaLojaRepository>();
            services.AddScoped<ILojaRepository, LojaRepository>();
            services.AddScoped<ITipoLojaRepository, TipoLojaRepository>();
            services.AddScoped<ILojaEnderecoRepository, LojaEnderecoRepository>();
            services.AddScoped<IPortifolioRepository, PortifolioRepository>();
            services.AddScoped<IPortifolioImagemRepository, PortifolioImagemRepository>();
            services.AddScoped<ILojaColaboradorRepository, LojaColaboradorRepository>();
            services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
            services.AddScoped<ILojaColaboradorRepository, LojaColaboradorRepository>();
            services.AddScoped<IServicosRepository, ServicoRepository>();
            services.AddScoped<IContaColaboradorRepository, ContaColaboradorRepository>();
            services.AddScoped<IColaboradorProfissionalRepository, ColaboradorProfissionalRepository>();
            services.AddScoped<ITipoPortifolioRepository, TipoPortifolioRepository>();
            services.AddScoped<IPortifolioContextRepository<LojaPortifolio, PortifolioDTO>, LojaPortifolioRepository>();
            services.AddScoped<IPortifolioContextRepository<ColaboradorPortifolio, PortifolioDTO>, ColaboradorPortifolioRepository>();
        }
    }
}
