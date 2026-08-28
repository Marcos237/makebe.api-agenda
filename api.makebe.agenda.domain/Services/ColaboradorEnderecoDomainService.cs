using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ColaboradorEnderecoDomainService : IColaboradorEnderecoDomainService
    {
        private readonly IColaboradorEnderecoRepository _colaboradorEnderecoRepository;

        public ColaboradorEnderecoDomainService(IColaboradorEnderecoRepository colaboradorEnderecoRepository)
        {
            _colaboradorEnderecoRepository = colaboradorEnderecoRepository;
        }

        public async Task<PaginacaoDTO<EnderecoDTO>> BuscarEndereco(PaginacaoDTO<EnderecoDTO> paginacao, string contaId)
        {
            return await _colaboradorEnderecoRepository.BuscarEnderecos(paginacao, contaId);
        }

        public async Task<int> Salvar(ColaboradorEndereco item)
        {
            item.Status = true;
            if (item.Id == 0)
            {
                item.DataCadastro = DateTime.Now;
                var response = await _colaboradorEnderecoRepository.Salvar(item);
                return response;
            }
            await _colaboradorEnderecoRepository.Atualizar(item);
            return item.Id;
        }
    }
}
