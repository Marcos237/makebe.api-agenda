using api.makebe.agenda.infra.data.Repositorys;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Domains
{
    public class RepositoryBootstrapper
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddTransient<IEnderecoRepository, EnderecoRepository>();
            services.AddTransient<ILojaRepository, LojaRepository>();
            services.AddTransient<IUsuarioLojaRepository, UsuarioLojaRepository>();
            services.AddTransient<ILojaRepository, LojaRepository>();
            services.AddTransient<IUsuarioSessaoRedisRepository, UsuarioSessaoRedisRepository>();
        }
    }
}
