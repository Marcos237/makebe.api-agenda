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
    [Route("LojaPortifolio")]
    [Authorize]
    public class LojaPortifolioController : BaseController
    {
        private readonly ILojaPortifolioApplicationService _lojaPortifolioApplicationService;
        public LojaPortifolioController(ILojaPortifolioApplicationService lojaPortifolioApplicationService)
        {
            _lojaPortifolioApplicationService = lojaPortifolioApplicationService;
        }
        [HttpPost]
        [Route("BuscarPaginado")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarPaginado(PaginacaoDTO<LojaPortifolioDTO> model)
        {
            try
            {
                var retorno = await _lojaPortifolioApplicationService.BuscarLojaPortifolios(model, Chave ?? string.Empty);
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
                var retorno = await _lojaPortifolioApplicationService.BuscarPorId(id);
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
        public async Task<IActionResult> Post(LojaPortifolioPayload model)
        {
            try
            {

                var retorno = await _lojaPortifolioApplicationService.Persistir(model, Chave ?? string.Empty);
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
        public async Task<IActionResult> Put(LojaPortifolioPayload model)
        {
            try
            {

                var retorno = await _lojaPortifolioApplicationService.Persistir(model, Chave ?? string.Empty);
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
                var retorno = await _lojaPortifolioApplicationService.Desativar(id, Chave ?? string.Empty);
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
