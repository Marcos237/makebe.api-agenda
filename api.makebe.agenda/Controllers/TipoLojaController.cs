using api.makebe.agenda.applications.Filters.Authorization;
using api.makebe.agenda.applications.Interfaces;
using lib.makebe.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    [ApiController]
    [Route("TipoLoja")]
    //[Authorize]
    public class TipoLojaController : BaseController
    {
        private readonly ITipoLojaApplicationService _tipoLojaApplicationService;
        public TipoLojaController(ITipoLojaApplicationService tipoLojaApplicationService)
        {
            _tipoLojaApplicationService = tipoLojaApplicationService;
        }

        [HttpGet]
        //[AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var retorno = await _tipoLojaApplicationService.BuscarTodos();
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
