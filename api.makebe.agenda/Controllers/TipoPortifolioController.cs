using api.makebe.agenda.applications.Filters.Authorization;
using api.makebe.agenda.applications.Interfaces;
using lib.makebe.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    [ApiController]
    [Route("TipoPortifolio")]
    [Authorize]
    public class TipoPortifolioController : BaseController
    {
        private readonly ITipoPortifolioApplicationService _tipoPortifolioApplicationService;
        public TipoPortifolioController(ITipoPortifolioApplicationService tipoPortifolioApplicationService)
        {
            _tipoPortifolioApplicationService = tipoPortifolioApplicationService;
        }

        [HttpGet("{id}")]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var retorno = await _tipoPortifolioApplicationService.BuscarPorTipoUsuarioId(id);
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
    }
}
