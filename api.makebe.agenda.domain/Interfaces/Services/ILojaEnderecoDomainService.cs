using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface ILojaEnderecoDomainService
    {
        Task<int> Salvar(LojaEndereco endereco);
        Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao, string contaId);
        Task<PaginacaoDTO<EnderecoDTO>> Filtrar(PaginacaoDTO<EnderecoDTO> paginacao);
    }
}
