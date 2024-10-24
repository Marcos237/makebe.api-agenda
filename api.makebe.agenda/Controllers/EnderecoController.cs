using api.makebe.agenda.applications.Filters.Authorization;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using lib.makebe.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    [ApiController]
    [Route("Endereco")]
    [Authorize]
    public class EnderecoController : BaseController
    {

        private readonly IEnderecoApplicationService _enderecoApplicationService;

        public EnderecoController(IEnderecoApplicationService enderecoApplicationService)
        {
            _enderecoApplicationService = enderecoApplicationService;
        }

        [HttpPost]
        [Route("BuscarPaginado")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarPaginado(PaginacaoDTO<EnderecoDTO> model)
        {
            try
            {
                var retorno = await _enderecoApplicationService.BuscarTodos(model, Chave ?? string.Empty);
                if (retorno.data == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, retorno);
                }
                return StatusCode(StatusCodes.Status200OK, retorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var retorno = await _enderecoApplicationService.BuscarPorId(id);
                if (retorno.data == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, retorno);
                }
                return StatusCode(StatusCodes.Status200OK, retorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Post(EnderecoDTO model)
        {
            try
            {
                var retorno = await _enderecoApplicationService.Persistir(model, Chave ?? string.Empty);
                if (retorno?.data?.Id == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, retorno);
                }
                return StatusCode(StatusCodes.Status200OK, retorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPut]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Put(EnderecoDTO model)
        {
            try
            {

                var retorno = await _enderecoApplicationService.Persistir(model, Chave ?? string.Empty);
                if (retorno.datas == null || !retorno.datas.Any())
                {
                    return StatusCode(StatusCodes.Status400BadRequest, retorno);
                }
                return StatusCode(StatusCodes.Status200OK, retorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var retorno = await _enderecoApplicationService.DesativarEnderecos(id);
                if (!retorno)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, retorno);
                }
                return StatusCode(StatusCodes.Status200OK, retorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
