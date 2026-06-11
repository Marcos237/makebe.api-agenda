using api.makebe.agenda.applications.Consumers;
using api.makebe.agenda.infra.crosscutting.Entidades.Constants;
using api.makebe.agenda.infra.crosscutting.Events;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using AgendamentoPersistenciaEvent;
using ColaboradorAgendamentoEvent;
using DesativarAgendamentoEvent;
using MassTransit;
using MeusAgendamentosEvent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PesquisarVitrineEvent;
using PeriodoDisponivelAgendamentoEvent;
using System.Net.Mime;


namespace api.makebe.agenda.infra.crosscutting.ioc.Infrastructure.Events
{
    public static class InfraEventBootstrapper
    {
        public static void InitializeInfraEventBootstrapper(this IServiceCollection services)
        {
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<LojasVitrinePublicadasConsumer>();
                busConfigurator.AddConsumer<ColaboradorProfissionalPublicadoConsumer>();
                busConfigurator.AddConsumer<ColaboradorPortifolioImagemPublicadoConsumer>();
                busConfigurator.AddConsumer<EnderecoLojaConsumer>();
                busConfigurator.AddConsumer<ColaboradorAgendamentoConsumer>();
                busConfigurator.AddConsumer<PeriodoDisponivelAgendamentoConsumer>();
                busConfigurator.AddConsumer<AgendamentoPersistidoConsumer>();
                busConfigurator.AddConsumer<MeusAgendamentosConsumer>();
                busConfigurator.AddConsumer<DesativarAgendamentoConsumer>();
                busConfigurator.AddConsumer<PesquisarVitrineConsumer>();

                busConfigurator.UsingRabbitMq((context, configuracao) =>
                {
                    var configuration = context.GetRequiredService<IConfiguration>();
                    configuracao.DefaultContentType = new ContentType("application/json");
                    configuracao.UseRawJsonDeserializer();

                    configuracao.Host(new Uri(configuration[RabbitMQConstant.HostName] ?? string.Empty), host =>
                    {
                        host.Username(configuration[RabbitMQConstant.User] ?? string.Empty);
                        host.Password(configuration[RabbitMQConstant.Senha] ?? string.Empty);
                    });

                    configuracao.ReceiveEndpoint("lojas-vitrine-publicadas-queue", e =>
                    {
                        e.ConfigureConsumer<LojasVitrinePublicadasConsumer>(context);
                    });

                    configuracao.ReceiveEndpoint("colaborador-profissional-publicado-queue", e =>
                    {
                        e.ConfigureConsumer<ColaboradorProfissionalPublicadoConsumer>(context);
                    });

                    configuracao.ReceiveEndpoint("colaborador-portifolio-imagem-publicado-queue", e =>
                    {
                        e.ConfigureConsumer<ColaboradorPortifolioImagemPublicadoConsumer>(context);
                    });

                    configuracao.ReceiveEndpoint("endereco-loja-publicado-queue", e =>
                    {
                        e.ConfigureConsumer<EnderecoLojaConsumer>(context);
                    });

                    configuracao.ReceiveEndpoint("colaborador-agendamento-publicado-queue", e =>
                    {
                        e.ConfigureConsumer<ColaboradorAgendamentoConsumer>(context);
                    });

                    configuracao.ReceiveEndpoint("periodo-disponivel-agendamento-publicado-queue", e =>
                    {
                        e.ConfigureConsumer<PeriodoDisponivelAgendamentoConsumer>(context);
                    });

                    configuracao.ReceiveEndpoint("agendamento-persistido-queue", e =>
                    {
                        e.ConfigureConsumer<AgendamentoPersistidoConsumer>(context);
                    });

                    configuracao.ReceiveEndpoint("meus-agendamentos-publicado-queue", e =>
                    {
                        e.ConfigureConsumer<MeusAgendamentosConsumer>(context);
                    });

                    configuracao.ReceiveEndpoint("desativar-agendamento-queue", e =>
                    {
                        e.ConfigureConsumer<DesativarAgendamentoConsumer>(context);
                    });

                    configuracao.ReceiveEndpoint("pesquisar-vitrine-queue", e =>
                    {
                        e.ConfigureConsumer<PesquisarVitrineConsumer>(context);
                    });
                });
            });
            services.AddScoped<IBusEvent, BusEvent>();
        }
    }
}
