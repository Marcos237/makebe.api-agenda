using api.makebe.agenda.domain.Interfaces.Repositorys;
using EnderecoLojaEvent;
using MassTransit;

namespace api.makebe.agenda.applications.Consumers
{
    public class EnderecoLojaConsumer
        : IConsumer<EnderecoLojaPublicadoEvent>
    {
        private readonly IEnderecoLojaRepository _lojaEnderecoRepository;

        public EnderecoLojaConsumer(IEnderecoLojaRepository lojaEnderecoRepository)
        {
            _lojaEnderecoRepository = lojaEnderecoRepository;
        }

        public async Task Consume(ConsumeContext<EnderecoLojaPublicadoEvent> context)
        {
            var message = context.Message;

            var lojas = await _lojaEnderecoRepository.BuscarEnderecoLoja(message.Id)
                ?? Enumerable.Empty<api.makebe.agenda.domain.DTO.EnderecoLojaDTO>();

            await context.RespondAsync(new EnderecoLojaPublicadoEvent
            {
                Id = message.Id,
                DataEvento = DateTime.Now,
                Lojas = lojas.Select(x => new EnderecoLojaEvent.EnderecoLojaEvent
                {
                    Id = x.Id,
                    RazaoSocial = x.RazaoSocial,
                    Telefone = x.Telefone,
                    Email = x.Email,
                    Texto = x.Texto,
                    CEP = x.CEP,
                    Cidade = x.Cidade,
                    Logradouro = x.Logradouro,
                    Numero = x.Numero,
                    Estado = x.Estado,
                    Complemento = x.Complemento,
                    Status = x.Status
                })
            });
        }
    }
}
