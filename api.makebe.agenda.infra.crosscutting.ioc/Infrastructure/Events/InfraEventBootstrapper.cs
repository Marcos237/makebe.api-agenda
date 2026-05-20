using api.makebe.agenda.applications.Consumers;
using api.makebe.agenda.infra.crosscutting.Entidades.Constants;
using api.makebe.agenda.infra.crosscutting.Events;
using api.makebe.agenda.infra.crosscutting.Events.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                busConfigurator.AddConsumer<EnderecoLojaConsumer>();

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

                    configuracao.ReceiveEndpoint("endereco-loja-publicado-queue", e =>
                    {
                        e.ConfigureConsumer<EnderecoLojaConsumer>(context);
                    });
                });
            });
            services.AddScoped<IBusEvent, BusEvent>();
        }
    }
}
