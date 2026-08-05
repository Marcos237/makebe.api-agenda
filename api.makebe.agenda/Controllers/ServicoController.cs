using api.makebe.agenda.applications.Filters.Authorization;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using api.makebe.agenda.domain.Entidades;
using lib.makebe.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    [ApiController]
    [Route("Servico")]
    [Authorize]
    public class ServicoController : BaseController
    {
        private readonly IServicoApplicationService _servicoApplicationService;
        public ServicoController(IServicoApplicationService servicoApplicationService)
        {
            _servicoApplicationService = servicoApplicationService;
        }
        [HttpGet]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Get()
        {

            var retorno = await _servicoApplicationService.BuscarServicos(Chave ?? string.Empty);
            if (!retorno.datas!.Any())
            {
                return StatusCode(StatusCodes.Status204NoContent, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);
        }

        [HttpPost]
        [Route("BuscarPaginado")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarPaginado(PaginacaoDTO<ServicoDTO> model)
        {
            var retorno = await _servicoApplicationService.BuscarTodosPaginado(model, Chave ?? string.Empty);
            if (retorno.data == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);

        }

        [HttpGet("{id}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Get(int id)
        {
            var retorno = await _servicoApplicationService.BuscarPorId(id);
            if (retorno.data == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);

        }

        [HttpGet("Categoria")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarCategorias()
        {
            var retorno = await _servicoApplicationService.BuscarCategorias();
            if (retorno.datas == null || !retorno.datas.Any())
            {
                return StatusCode(StatusCodes.Status204NoContent, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);
        }

        [HttpGet("GetByColaboradorId/{id}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> GetByColaboradorId(int id)
        {

            var retorno = await _servicoApplicationService.BuscarServicosPorColaboradorId(id);
            if (retorno?.datas?.Any() == false)
            {
                return StatusCode(StatusCodes.Status204NoContent, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);

        }

        [HttpPost]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Post(ServicoDTO model)
        {
            var retorno = await _servicoApplicationService.Persitir(model, Chave ?? string.Empty);
            if (retorno?.data?.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);

        }
        [HttpDelete]
        [Route("{id}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Delete(int id)
        {

            var retorno = await _servicoApplicationService.Desativar(id, Chave ?? string.Empty);
            if (!retorno)
            {
                return StatusCode(StatusCodes.Status400BadRequest, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);
        }
    }
}
