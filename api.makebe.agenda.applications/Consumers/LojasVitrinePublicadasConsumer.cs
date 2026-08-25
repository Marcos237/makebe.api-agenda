using api.makebe.agenda.domain.Constants;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using LojasEvent;
using MassTransit;

namespace api.makebe.agenda.applications.Consumers
{
    public class LojasVitrinePublicadasConsumer
        : IConsumer<LojasVitrinePublicadasEvent>
    {
        private readonly ILojaRepository _lojaRepository;

        public LojasVitrinePublicadasConsumer(
            ILojaRepository lojaRepository)
        {
            _lojaRepository = lojaRepository;
        }

        public async Task Consume(ConsumeContext<LojasVitrinePublicadasEvent> context)
        {
            var message = context.Message;

            var lojasVitrine = Enumerable.Empty<LojaVitrineDTO>();
            if (message.Tipo == LojaConstants.TipoLojaVitrine)
                lojasVitrine = await _lojaRepository.BuscarLojasVitrinePorTipo(message.Tipo);

            else
             lojasVitrine = await _lojaRepository.BuscarLojasBannerPorTipo(message.Tipo);

            await context.RespondAsync(new LojasVitrinePublicadasEvent
            {
                Tipo = message.Tipo,
                DataEvento = DateTime.Now,
                Lojas = lojasVitrine.Select(x => new LojaVitrineEvent
                {
                    Id = x.Id,
                    RazaoSocial = x.RazaoSocial,
                    NomeImagem = x.NomeImagem,
                    TituloImagem = x.TituloImagem,
                    UrlImagem = x.UrlImagem
                }).ToList()
            });
        }
    }
}