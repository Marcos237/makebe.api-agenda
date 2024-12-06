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
            services.AddScoped<IUsuarioLojaRepository, UsuarioLojaRepository>();
            services.AddScoped<ILojaRepository, LojaRepository>();
            services.AddScoped<ITipoLojaRepository, TipoLojaRepository>();
            services.AddScoped<ILojaEnderecoRepository, LojaEnderecoRepository>();
            services.AddScoped<ILojaPortifolioRepository, LojaPortifolioRepository>();
            services.AddScoped<ILojaPortifolioImagemRepository, LojaPortifolioImagemRepository>();
            services.AddScoped<ILojaColaboradorRepository, LojaColaboradorRepository>();
            services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
            services.AddScoped<ILojaColaboradorRepository, LojaColaboradorRepository>();
            services.AddScoped<IServicosRepository, ServicoRepository>();
        }
    }
}
