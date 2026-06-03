using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.infra.crosscutting.Services.Interfaces;
using ColaboradoresProfissionalEvent;
using MassTransit;
using UsuariosEvent;

namespace api.makebe.agenda.applications.Consumers
{
    public class ColaboradorPortifolioImagemPublicadoConsumer
        : IConsumer<ColaboradorPortifolioImagemPublicadoEvent>
    {
        private readonly IPortifolioImagemRepository _portifolioImagemRepository;
        private readonly IUsuarioEventCrossCuttingService _usuarioEventCrossCuttingService;

        public ColaboradorPortifolioImagemPublicadoConsumer(IPortifolioImagemRepository portifolioImagemRepository, IUsuarioEventCrossCuttingService usuarioEventCrossCuttingService)
        {
            _portifolioImagemRepository = portifolioImagemRepository;
            _usuarioEventCrossCuttingService = usuarioEventCrossCuttingService; 
        }

        public async Task Consume(ConsumeContext<ColaboradorPortifolioImagemPublicadoEvent> context)
        {
            var message = context.Message;
            var imagens = await _portifolioImagemRepository.BuscarImagensPorColaboradorId(message.Id);
            var usuarioId = imagens.FirstOrDefault()?.UsuarioId;    
            var usuarioEvent = new UsuarioConsultadoPorIdEvent() { Id = PropiedadesHelper.ParseGuidOrDefault(usuarioId) };
            var usuario = await _usuarioEventCrossCuttingService.BuscarUsuarioPorId(usuarioEvent);  
            imagens.Select(x => x.NomeColaborador = usuario?.UsuarioConsultadoRetorno?.Nome ?? x.NomeColaborador);

            await context.RespondAsync(new ColaboradorPortifolioImagemPublicadoEvent
            {
                Id = message.Id,
                DataEvento = DateTime.UtcNow,
                Imagens = imagens.Select(x => new ColaboradorPortifolioImagemEvent
                {
                    NomeImagem = x.NomeImagem,
                    UrlImagem = x.UrlImagem,
                    TituloImagem = x.TituloImagem,
                    ColaboradorId = x.ColaboradorId,
                    UsuarioId = x.UsuarioId,
                    NomeColaborador = usuario?.UsuarioConsultadoRetorno?.Nome,
                    Telefone = usuario?.UsuarioConsultadoRetorno?.Telefone,
                    Email = usuario?.UsuarioConsultadoRetorno?.Email,
                    Texto = x.Texto
                })
            });
        }
    }
}
