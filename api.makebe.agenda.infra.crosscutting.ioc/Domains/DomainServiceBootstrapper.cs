using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Services.Portifolios;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces;
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
            services.AddScoped<IUsuarioPermissaoDomainService, UsuarioPermissaoDomainService>();
            services.AddScoped<ILojaColaboradorDomainService, LojaColaboradorDomainService>();
            services.AddScoped<IServicosDomainService, ServicoDomainService>();
            services.AddScoped<ICategoriaDomainService, CategoriaDomainService>();
            services.AddScoped<ICategoriaItemDomainService, CategoriaItemDomainService>();
            services.AddScoped<IContaColaboradorDomainService, ContaColaboradorDomainService>();
            services.AddScoped<IColaboradorProfissionalDomainService, ColaboradorProfissionalDomainService>();
            services.AddScoped<ITipoPortifolioDomainService, TipoPortifolioDomainService>();
            services.AddScoped<ILojaPortifolioDomainService, LojaPortifolioDomainService>();
            services.AddScoped<IColaboradorPortifolioDomainService, ColaboradorPortifolioDomainService>();
            services.AddScoped<IContaServicoDomainService, ContaServicoDomainService>();
            services.AddScoped<IAgendaDomainService, AgendaDomainService>();
            services.AddScoped<IAgendaContextDomainService<AgendaLoja>, AgendaLojaDomainService>();
            services.AddScoped<IAgendaContextDomainService<AgendaColaborador>, AgendaColaboradorDomainService>();
            services.AddScoped<IAgendaColaboradorDomainService, AgendaColaboradorDomainService>();
            services.AddScoped<IFiltrosAgendamentoDomainService, FiltrosAgendamentoDomainService>();
            services.AddScoped<IPortifolioValidacaoAplicationService, PortifolioValidacaoAplicationService>();
            services.AddScoped<IAgendamentoDomainService, AgendamentoDomainService>();
            services.AddScoped<IEmailEnvioDomainService, EmailEnvioDomainService>();
            services.AddScoped<IAgendamentoColaboradorDomainService, AgendamentoColaboradorDomainServices>();
            services.AddScoped<IPeriodoDisponivelAgendamentoDomainService, PeriodoDisponivelAgendamentoDomainService>();


            services.AddScoped<IValidationService<Loja>, ValidationService<Loja>>();
            services.AddScoped<IValidationService<Endereco>, ValidationService<Endereco>>();
            services.AddScoped<IValidationService<LojaEndereco>, ValidationService<LojaEndereco>>();
            services.AddScoped<IValidationService<ColaboradorEndereco>, ValidationService<ColaboradorEndereco>>();
            services.AddScoped<IValidationService<Portifolio>, ValidationService<Portifolio>>();
            services.AddScoped<IValidationService<Arquivo>, ValidationService<Arquivo>>();
            services.AddScoped<IValidationService<ColaboradorProfissional>, ValidationService<ColaboradorProfissional>>();
            services.AddScoped<IValidationService<Servico>, ValidationService<Servico>>();
            services.AddScoped<IValidationService<Agenda>, ValidationService<Agenda>>();
            services.AddScoped<IValidationService<LojaPortifolio>, ValidationService<LojaPortifolio>>();
            services.AddScoped<IValidationService<ColaboradorPortifolio>, ValidationService<ColaboradorPortifolio>>();
            services.AddScoped<IValidationService<AgendaLoja>, ValidationService<AgendaLoja>>();
            services.AddScoped<IValidationService<AgendaColaborador>, ValidationService<AgendaColaborador>>();
            services.AddScoped<IValidationService<AgendamentoDTO>, ValidationService<AgendamentoDTO>>();

            services.AddScoped<IValidator<Loja>, LojaValidation>();
            services.AddScoped<IValidator<Endereco>, EnderecoValidation>();
            services.AddScoped<IValidator<ContaLoja>, UsuarioLojaValidation>();
            services.AddScoped<IValidator<LojaEndereco>, LojaEnderecoValidation>();
            services.AddScoped<IValidator<ColaboradorEndereco>, ColaboradorEnderecoValidation>();
            services.AddScoped<IValidator<Portifolio>, PortifolioValidation>();
            services.AddScoped<IValidator<Arquivo>, ArquivoValidation>();
            services.AddScoped<IValidator<ColaboradorProfissional>, ColaboradorProfissionalValidation>();
            services.AddScoped<IValidator<Servico>, ServicoValidation>();
            services.AddScoped<IValidator<Agenda>, AgendaValidation>();
            services.AddScoped<IValidator<LojaPortifolio>, LojaPortifolioValidation>();
            services.AddScoped<IValidator<ColaboradorPortifolio>, ColaboradorPortifolioValidation>();
            services.AddScoped<IValidator<AgendaLoja>, AgendaLojaValidation>();
            services.AddScoped<IValidator<AgendaColaborador>, AgendaColaboradorValidation>();
            services.AddScoped<IValidator<AgendamentoDTO>, AgendamentoValidation>();

        }
    }
}
