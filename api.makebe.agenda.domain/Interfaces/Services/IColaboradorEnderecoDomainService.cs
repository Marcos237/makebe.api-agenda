using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Services
{
    public interface IColaboradorEnderecoDomainService
    {
        Task<PaginacaoDTO<EnderecoDTO>> BuscarEndereco(PaginacaoDTO<EnderecoDTO> paginacao, string contaId);
        Task<int> Salvar(ColaboradorEndereco item);
    }
}
