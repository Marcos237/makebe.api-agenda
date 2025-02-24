using api.makebe.agenda.domain.DTO;

public interface IEnderecoBuscaStrategyContext
{
    Task<PaginacaoDTO<EnderecoDTO>> Buscar(PaginacaoDTO<EnderecoDTO> paginacao, string contaId);
}
