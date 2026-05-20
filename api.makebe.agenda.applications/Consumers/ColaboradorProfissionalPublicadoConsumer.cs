using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using ColaboradoresProfissionalEvent;
using MassTransit;
using UsuariosEvent;

namespace api.makebe.agenda.applications.Consumers
{
    public class ColaboradorProfissionalPublicadoConsumer
        : IConsumer<ColaboradorProfissionalPublicadoEvent>
    {
        private readonly IColaboradorProfissionalRepository _colaboradorProfissionalRepository;
        private readonly IUsuarioEventCrossCuttingService _usuarioEventCrossCuttingService;

        public ColaboradorProfissionalPublicadoConsumer(
            IColaboradorProfissionalRepository colaboradorProfissionalRepository,
            IUsuarioEventCrossCuttingService usuarioEventCrossCuttingService)
        {
            _colaboradorProfissionalRepository = colaboradorProfissionalRepository;
            _usuarioEventCrossCuttingService = usuarioEventCrossCuttingService;

        }

        public async Task Consume(ConsumeContext<ColaboradorProfissionalPublicadoEvent> context)
        {
            var message = context.Message;

            var colaboradoresProfissionais = Enumerable.Empty<ColaboradorProfissionalEvent>();

            if (message.LojaId > 0)
            {
                var colaboradores = await _colaboradorProfissionalRepository.BuscarPorLojaId(message.LojaId);

                foreach (var colaborador in colaboradores)
                {
                    if (!string.IsNullOrEmpty(colaborador.UsuarioId))
                    {
                        var usuarioEvent = new UsuarioConsultadoPorIdEvent() { Id = PropiedadesHelper.ParseGuidOrDefault(colaborador.UsuarioId) };
                        var usuario = await _usuarioEventCrossCuttingService.BuscarUsuarioPorId(usuarioEvent);
                        colaborador.NomeColaborador = usuario?.UsuarioConsultadoRetorno?.Nome ?? colaborador.NomeColaborador;
                        colaborador.UrlImagem = usuario?.UsuarioConsultadoRetorno?.UrlImagem ?? colaborador.UrlImagem;
                    }
                }
                colaboradoresProfissionais = colaboradores.Select(x => new ColaboradorProfissionalEvent
                {
                    Id = x.Id,
                    ColaboradorId = x.ColaboradorId,
                    UsuarioId = x.UsuarioId,
                    Nome = x.NomeColaborador,
                    LojaId = x.LojaId,
                    ServicoId = x.ServicoId,
                    DescricaoServico = x.DescricaoServico,
                    UrlImagem = x.UrlImagem
                });
            }

            await context.RespondAsync(new ColaboradorProfissionalPublicadoEvent
            {
                LojaId = message.LojaId,
                DataEvento = DateTime.UtcNow,
                ColaboradoresProfissionais = colaboradoresProfissionais
            });
        }
    }
}
