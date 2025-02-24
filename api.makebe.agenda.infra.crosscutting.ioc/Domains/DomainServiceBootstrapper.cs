using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.domain.Services;
using api.makebe.agenda.domain.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace api.makebe.agenda.infra.crosscutting.ioc.Domains
{
    public static class DomainServiceBootstrapper
    {
        public static void InitializeDomainServiceBootstrapper(this IServiceCollection services)
        {
            services.AddScoped<IEnderecoDomainService, EnderecoDomainService>();
            services.AddScoped<ILojaDomainService, LojaDomainService>();
            services.AddScoped<IContaLojaDomainService, ContaLojaDomainService>();
            services.AddScoped<ITipoLojaDomainService, TipoLojaDomainService>();
            services.AddScoped<ILojaEnderecoDomainService, LojaEnderecoDomainService>();
            services.AddScoped<IColaboradorEnderecoDomainService, ColaboradorEnderecoDomainService>();
            services.AddScoped<IPortifolioDomainService, PortifolioDomainService>();
            services.AddScoped<IPortifolioImagemDomainService, PortifolioImagemDomainService>();
            services.AddScoped<IArquivoDomainService, ArquivoDomainService>();
            services.AddScoped<IColaboradorDomainService, ColaboradorDomainService>();
            services.AddScoped<ILojaColaboradorDomainService, LojaColaboradorDomainService>();
            services.AddScoped<IServicosDomainService, ServicoDomainService>();
            services.AddScoped<IContaColaboradorDomainService, ContaColaboradorDomainService>();
            services.AddScoped<IColaboradorProfissionalDomainService, ColaboradorProfissionalDomainService>();
            services.AddScoped<ITipoPortifolioDomainService, TipoPortifolioDomainService>();
            services.AddScoped<ILojaPortifolioDomainService, LojaPortifolioDomainService>();
            services.AddScoped<IColaboradorPortifolioDomainService, ColaboradorPortifolioDomainService>();

            services.AddScoped<IValidationService<Loja>, ValidationService<Loja>>();
            services.AddScoped<IValidationService<Endereco>, ValidationService<Endereco>>();
            services.AddScoped<IValidationService<LojaEndereco>, ValidationService<LojaEndereco>>();
            services.AddScoped<IValidationService<Portifolio>, ValidationService<Portifolio>>();
            services.AddScoped<IValidationService<Arquivo>, ValidationService<Arquivo>>();
            services.AddScoped<IValidationService<ColaboradorProfissional>, ValidationService<ColaboradorProfissional>>();

            services.AddScoped<IValidator<Loja>, LojaValidation>();
            services.AddScoped<IValidator<Endereco>, EnderecoValidation>();
            services.AddScoped<IValidator<ContaLoja>, UsuarioLojaValidation>();
            services.AddScoped<IValidator<LojaEndereco>, LojaEnderecoValidation>();
            services.AddScoped<IValidator<Portifolio>, PortifolioValidation>();
            services.AddScoped<IValidator<Arquivo>, ArquivoValidation>();
            services.AddScoped<IValidator<ColaboradorProfissional>, ColaboradorProfissionalValidation>();

        }
    }
}
