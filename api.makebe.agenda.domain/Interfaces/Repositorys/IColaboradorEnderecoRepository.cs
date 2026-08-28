using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.domain.Interfaces.Repositorys
{
    public interface IColaboradorEnderecoRepository
    {
        Task<PaginacaoDTO<EnderecoDTO>> BuscarEnderecos(PaginacaoDTO<EnderecoDTO> paginacao, string contaId);
        Task<int> Salvar(ColaboradorEndereco item);
        Task<bool> Atualizar(ColaboradorEndereco item);
    }
}
