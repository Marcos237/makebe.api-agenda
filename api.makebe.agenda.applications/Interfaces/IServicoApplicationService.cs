using api.makebe.agenda.applications.Models.Responses;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;

namespace api.makebe.agenda.applications.Interfaces
{
    public interface IServicoApplicationService
    {
        Task<ResponseModel<Servico>> BuscarServicos(string usuarioId);
        Task<ResponseModel<PaginacaoDTO<ServicoDTO>>> BuscarTodosPaginado(PaginacaoDTO<ServicoDTO> paginacaoDTO, string usuarioId);
        Task<ResponseModel<ServicoDTO>> BuscarPorId(int id);
        Task<ResponseModel<CategoriaItem>> BuscarCategorias();
        Task<ResponseModel<ServicoDTO>> BuscarServicosPorColaboradoId(int id);
        Task<ResponseModel<ServicoDTO>> Persitir(ServicoDTO item, string usuarioId);
        Task<bool> Desativar(int id, string usuarioId);

    }
}
