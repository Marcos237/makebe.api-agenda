using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.domain.Services;
using api.makebe.agenda.domain.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Domains
{
    public class DomainServiceBootstrapper
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddTransient<IEnderecoDomainService, EnderecoDomainService>();
            services.AddTransient<ILojaDomainService, LojaDomainService>();
            services.AddTransient<IUsuarioLojaDomainService, UsuarioLojaDomainService>();
            services.AddTransient<ITipoLojaDomainService, TipoLojaDomainService>();

            services.AddTransient<IValidationService<Loja>, ValidationService<Loja>>();

            services.AddScoped<IValidator<Loja>, LojaValidation>();
            services.AddScoped<IValidator<Endereco>, EnderecoValidation>();
            services.AddScoped<IValidator<UsuarioLoja>, UsuarioLojaValidation>();

        }
    }
}
