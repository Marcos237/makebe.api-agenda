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
        public async Task<PaginacaoDTO<EnderecoDTO>> BuscarTodos(PaginacaoDTO<EnderecoDTO> paginacao, string usuarioId)
        {
            var result = await _enderecoRepository.BuscarEnderecos(paginacao, usuarioId);
            result.totalPaginas = (result.total + result.quantidadePagina - 1) / result.quantidadePagina;
            return result;
        }
        public async Task<EnderecoDTO> BuscarPorId(int id)
        {
            var resul = await _enderecoRepository.BuscarPorId(id);
            return resul;
        }
        public async Task<int> Salvar(Endereco item)
        {
            item.Status = true;
            item.DataAtualizacao = DateTime.Now;
            if (item.Id == 0)
            {
                item.DataCadastro = DateTime.Now;
                var result = await _enderecoRepository.Salvar(item);
                return result;
            }

            var resultAualizado = await _enderecoRepository.Atualizar(item);
            return resultAualizado.Id;
        }

        public async Task<Endereco> Atualizar(Endereco item)
        {
            var result = await _enderecoRepository.Atualizar(item);
            return result;
        }

        public async Task<bool> Desativar(int id)
        {
            var result = await _enderecoRepository.Deastivar(id);
            return result;

        }
    }
}
