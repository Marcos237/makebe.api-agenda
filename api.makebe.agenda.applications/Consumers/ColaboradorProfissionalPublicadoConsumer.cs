using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;
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
        private readonly IColaboradorProfissionalDomainService _colaboradorProfissionalDomainService;
        private readonly IUsuarioEventCrossCuttingService _usuarioEventCrossCuttingService;

        public ColaboradorProfissionalPublicadoConsumer(
            IColaboradorProfissionalRepository colaboradorProfissionalRepository,
            IColaboradorProfissionalDomainService colaboradorProfissionalDomainService,
            IUsuarioEventCrossCuttingService usuarioEventCrossCuttingService)
        {
            _colaboradorProfissionalRepository = colaboradorProfissionalRepository;
            _colaboradorProfissionalDomainService = colaboradorProfissionalDomainService;
            _usuarioEventCrossCuttingService = usuarioEventCrossCuttingService;

        }

        public async Task Consume(ConsumeContext<ColaboradorProfissionalPublicadoEvent> context)
        {
            var message = context.Message;

            var colaboradoresProfissionais = new List<ColaboradorProfissionalEvent>();

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
                        var isAgendaVisible = await _colaboradorProfissionalDomainService.BuscarAgendaVisible(colaborador.ColaboradorId);
                        colaboradoresProfissionais.Add(new ColaboradorProfissionalEvent()
                        {
                            NomeColaborador = usuario?.UsuarioConsultadoRetorno?.Nome,
                            UrlImagem = colaborador.UrlImagem,
                            ColaboradorId = colaborador.ColaboradorId,  
                            DescricaoServico = colaborador.DescricaoServico,
                            Id = colaborador.Id,
                            LojaId = colaborador.LojaId,
                            ServicoId = colaborador.ServicoId,
                            UsuarioId = colaborador.UsuarioId,
                            Texto = colaborador.Texto,
                            IsAgendaVisible = isAgendaVisible
                        });
                            
                    }
                }
            }

            await context.RespondAsync(new ColaboradorProfissionalPublicadoEvent
            {
                LojaId = message.LojaId,
                DataEvento = DateTime.Now,
                ColaboradoresProfissionais = colaboradoresProfissionais
            });
        }
    }
}
