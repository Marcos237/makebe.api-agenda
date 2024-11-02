using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.infra.data.Repositorys;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Domains
{
    public class RepositoryBootstrapper
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<ILojaRepository, LojaRepository>();
            services.AddScoped<IUsuarioLojaRepository, UsuarioLojaRepository>();
            services.AddScoped<ILojaRepository, LojaRepository>();
            services.AddScoped<ITipoLojaRepository, TipoLojaRepository>();
            services.AddScoped<ILojaEnderecoRepository, LojaEnderecoRepository>();
            services.AddScoped<ILojaPortifolioRepository, LojaPortifolioRepository>();
            services.AddScoped<ILojaPortifolioImagemRepository, LojaPortifolioImagemRepository>();
        }
    }
}
