using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using api.makebe.agenda.domain.Helpers;
using api.makebe.agenda.domain.Interfaces.Repositorys;
using api.makebe.agenda.domain.Interfaces.Services;

namespace api.makebe.agenda.domain.Services
{
    public class ServicoDomainService : IServicosDomainService
    {
        private readonly IServicosRepository _servicosRepository;
        public ServicoDomainService(IServicosRepository servicosRepository)
        {
            _servicosRepository = servicosRepository;   
        }

        public async Task<IEnumerable<Servicos>> BuscarServicos(string contaId)
        {
            return await _servicosRepository.BuscarServicos(contaId);
        }

        public async  Task<PaginacaoDTO<ServicoDTO>> BuscarTodosPaginado(PaginacaoDTO<ServicoDTO> paginacao, string usuarioId)
        {
            var result = await _servicosRepository.BuscarPaginado(paginacao, usuarioId);
            result.totalPaginas = (result.total + result.quantidadePagina - 1) / result.quantidadePagina;
            foreach (var servico in result?.objetos!)
            {
                servico.ValorExtenso = ValoresHelper.SetValorExtenso(servico.Valor);
                servico.PeriodoExtenso = ValoresHelper.SetPeridoExtenso(servico.Periodo);
            }
            return result;
        }

        public async Task<Servicos> BuscarPorId(int id)
        {
            var response = await _servicosRepository.BuscarPorId(id);
            return response;
        }

        public async Task<int> Persitir(Servicos item)
        {
            item.Status = true;
            item.DataAtualizacao = DateTime.Now;
            if(item.Id == 0)
            {
                item.DataCadastro = DateTime.Now;
                var response = await _servicosRepository.Salvar(item);
                return response;
            }
            var resposeUpdate = await _servicosRepository.Atualizar(item);
            return resposeUpdate.Id;
        }
        public async Task<bool> Desativar(int id)
        {
            var response = await _servicosRepository.Desativar(id); 
            return response;    
        }
    }
}
