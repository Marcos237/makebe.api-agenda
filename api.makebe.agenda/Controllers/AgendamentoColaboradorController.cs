using api.makebe.agenda.applications.Filters.Authorization;
using api.makebe.agenda.applications.Interfaces;
using lib.makebe.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    [ApiController]
    [Route("AgendamentoColaborador")]
    [Authorize]
    public class AgendamentoColaboradorController : BaseController
    {
        private readonly IAgendamentoColaboradorApplicationService _service;
        public AgendamentoColaboradorController(IAgendamentoColaboradorApplicationService service)
        {
            _service = service;
        }
        [HttpGet]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var retorno = await _service.BuscarColaboladoresPorConta(Chave ?? string.Empty);
                if (!retorno.datas!.Any())
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
    }
}
