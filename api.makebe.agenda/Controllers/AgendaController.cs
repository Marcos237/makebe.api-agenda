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
    [Route("Agenda")]
    [Authorize]
    public class AgendaController : BaseController
    {
        private readonly IAgendaApplicationService _agendaLojaApplicationService;
        public AgendaController(IAgendaApplicationService agendaLojaApplicationService)
        {
            _agendaLojaApplicationService = agendaLojaApplicationService;
        }
        [HttpPost]
        [Route("BuscarPaginado")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarPaginado(PaginacaoDTO<AgendaPayload> model)
        {
            try
            {
                var retorno = await _agendaLojaApplicationService.BuscarTodosPaginado(model, Chave ?? string.Empty);
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

        [HttpGet("{id}/{tipo}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Get(int id, int tipo)
        {
            try
            {
                var retorno = await _agendaLojaApplicationService.BuscarPorId(id, tipo);
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
        public async Task<IActionResult> Post(AgendaPayload model)
        {
            try
            {

                var retorno = await _agendaLojaApplicationService.Persitir(model, Chave ?? string.Empty);
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

        [HttpDelete]
        [Route("{id}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var retorno = await _agendaLojaApplicationService.Desativar(id, Chave ?? string.Empty);
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
