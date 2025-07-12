using api.makebe.agenda.applications.Filters.Authorization;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.applications.Services;
using api.makebe.agenda.domain.DTO;
using lib.makebe.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    [ApiController]
    [Route("ColaboradorProfissional")]
    [Authorize]
    public class ColaboradorProfissionalController : BaseController
    {
        private readonly IColaboradorProfissionalApplicationService _ColaboradorProfissionalApplicationService;
        public ColaboradorProfissionalController(IColaboradorProfissionalApplicationService ColaboradorProfissionalApplicationService)
        {
            _ColaboradorProfissionalApplicationService = ColaboradorProfissionalApplicationService;
        }
        [HttpPost]
        [Route("BuscarPaginado")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarColaboradores(PaginacaoDTO<ColaboradorProfissionalDTO> model)
        {
            try
            {
                var retorno = await _ColaboradorProfissionalApplicationService.BuscarUsuariosPaginado(model, Chave ?? string.Empty);
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
                var retorno = await _ColaboradorProfissionalApplicationService.BuscarUsuarioPorId(id);
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

        [HttpGet("BuscarPorIdConta")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarPorIdConta()
        {
            try
            {
                var retorno = await _ColaboradorProfissionalApplicationService.BuscarPorConta(Chave ?? string.Empty);
                if (retorno.datas?.Any() == false)
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
        public async Task<IActionResult> Post(ColaboradorProfissionalPayload model)
        {
            try
            {
                var retorno = await _ColaboradorProfissionalApplicationService.Persistir(model);
                if (retorno.data == null)
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
                var retorno = await _ColaboradorProfissionalApplicationService.Desativar(id);
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
