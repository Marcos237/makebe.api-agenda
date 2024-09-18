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
    [Route("Loja")]
    [Authorize]
    public class LojaController : BaseController
    {
        private readonly ILojaApplicationService _lojaApplicationService;
        public LojaController(ILojaApplicationService lojaApplicationService)
        {
            _lojaApplicationService = lojaApplicationService;
        }
        [HttpGet]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Get(PaginacaoDTO<LojaPayload> model)
        {
            try
            {
                var retorno  = await _lojaApplicationService.BuscarTodos(model, Chave ?? string.Empty);
                if (retorno.datas == null || !retorno.datas.Any())
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
        public async Task<IActionResult> Post(LojaPayload model)
        {
            try
            {
                var retorno = await _lojaApplicationService.Persitir(model, Chave ?? string.Empty);
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
        [HttpPut]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Put(LojaPayload model)
        {
            try
            {
                var retorno = await _lojaApplicationService.Persitir(model, Chave ?? string.Empty);
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

        [HttpPut]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Delete(int  id)
        {
            try
            {
                var retorno = await _lojaApplicationService.Desativar(id, Chave ?? string.Empty);
                if (retorno)
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
