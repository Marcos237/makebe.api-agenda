using api.makebe.agenda.applications.Filters.Authorization;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using api.makebe.agenda.domain.DTO;
using lib.makebe.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    [ApiController]
    [Route("Portifolio")]
    [Authorize]
    public class PortifolioController : BaseController
    {
        private readonly IPortifolioApplicationService _portifolioApplicationService;
        public PortifolioController(IPortifolioApplicationService portifolioApplicationService)
        {
            _portifolioApplicationService = portifolioApplicationService;
        }
        [HttpPost]
        [Route("BuscarPaginado")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarPaginado(PaginacaoDTO<PortifolioDTO> model)
        {
            try
            {
                var retorno = await _portifolioApplicationService.BuscarPortifolios(model, Chave ?? string.Empty);
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
                var retorno = await _portifolioApplicationService.BuscarPorId(id);
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
        public async Task<IActionResult> Post(PortifolioPayload model)
        {
            try
            {

                var retorno = await _portifolioApplicationService.Persistir(model, Chave ?? string.Empty);
                if (retorno.notifications!.Any())
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
        public async Task<IActionResult> Put(PortifolioPayload model)
        {
            try
            {

                var retorno = await _portifolioApplicationService.Persistir(model, Chave ?? string.Empty);
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
        public async Task<IActionResult> Delete(int  id)
        {
            try
            {
                var retorno = await _portifolioApplicationService.Desativar(id, Chave ?? string.Empty);
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
