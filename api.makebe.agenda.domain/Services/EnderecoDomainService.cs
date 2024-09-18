using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Services;
using api.makebe.agenda.infra.crosscutting.Notifications.Interfaces;
using api.makebe.agenda.infra.data.Repositorys.Interfaces;
using FluentValidation;

namespace api.makebe.agenda.domain.Services
{
    public class EnderecoDomainService : IEnderecoDomainService
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IValidator<Endereco> _validator;
        private readonly INotificationContext _notificationContext;

        public EnderecoDomainService(IEnderecoRepository enderecoRepository, IValidator<Endereco> validator, INotificationContext notificationContext)
        {
            _enderecoRepository = enderecoRepository;
            _validator = validator;
            _notificationContext = notificationContext;
        }
        public async Task<IEnumerable<Endereco>> BuscarTodos(PaginacaoDTO<Endereco> paginacao, string usuarioId)
        {
            var resul = await _enderecoRepository.BuscarEnderecos(paginacao, usuarioId);
            return resul;
        }
        public async Task<Endereco> BuscarPorId(int id)
        {
            var resul = await _enderecoRepository.BuscarPorId(id);
            return resul;
        }
        public async Task<IEnumerable<Endereco>> BuscarPorLojaId(int id)
        {
            var resul = await _enderecoRepository.BuscarPorLojaId(id);
            return resul;
        }
        public async Task<int> Salvar(Endereco item)
        {
            var result = await _enderecoRepository.Salvar(item);
            return result;
        }

        public async Task<Endereco> Atualizar(Endereco item)
        {
            var result = await _enderecoRepository.Atualizar(item);
            return result;
        }

        public async Task<bool> Desativar(int id)
        {
            var endereco = await _enderecoRepository.BuscarPorId(id);
            endereco.Status = false;
            var result = await _enderecoRepository.Atualizar(endereco) != null;
            return result;

        }
    }
}
