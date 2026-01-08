using api.makebe.agenda.applications.Filters.Authorization;
using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.domain.DTO;
using lib.makebe.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    [ApiController]
    [Route("Agendamento")]
    [Authorize]
    public class AgendamentoController : BaseController
    {
        private readonly IAgendamentoApplicationService _agendamentoApplicationService;
        public AgendamentoController(IAgendamentoApplicationService agendamentoApplicationService)
        {
            _agendamentoApplicationService = agendamentoApplicationService;
        }
        [HttpGet("BuscarAgendamentos/{ano}/{id}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarAgendamentos(int ano, int id)
        {

            var retorno = await _agendamentoApplicationService.BuscarAgendamentoPorAno(ano, id, Chave ?? string.Empty);
            if (retorno.datas?.Any() == false)
            {
                return StatusCode(StatusCodes.Status204NoContent, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);

        }
        [HttpGet("BuscarAgendamentoPorData/{data}/{id}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarAgendamentoPorData(string data, int id)
        {

            var retorno = await _agendamentoApplicationService.BuscarAgendamentoPorData(data, id, Chave ?? string.Empty);
            if (retorno.datas?.Any() == false)
            {
                return StatusCode(StatusCodes.Status204NoContent, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);

        }
        [HttpGet("BuscarAgendamentoPorId/{id}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarAgendamentoPorid(int id)
        {

            var retorno = await _agendamentoApplicationService.BuscarAgendamentoPorId(id);
            if (retorno.datas?.Any() == false)
            {
                return StatusCode(StatusCodes.Status204NoContent, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);

        }

        [HttpPost]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Post(AgendamentoDTO model)
        {

            var retorno = await _agendamentoApplicationService.Persistir(model, Chave ?? string.Empty);
            if (retorno.data == null)
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

            var retorno = await _agendamentoApplicationService.Desativar(id);
            if (!retorno)
            {
                return StatusCode(StatusCodes.Status400BadRequest, retorno);
            }
            return StatusCode(StatusCodes.Status200OK, retorno);

        }
    }
}
