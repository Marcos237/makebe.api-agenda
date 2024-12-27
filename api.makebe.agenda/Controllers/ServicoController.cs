using api.makebe.agenda.applications.Filters.Authorization;
using api.makebe.agenda.applications.Interfaces;
using lib.makebe.domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    [ApiController]
    [Route("Servico")]
    [Authorize]
    public class ServicoController : BaseController
    {
        private readonly IServicoApplicationService _servicoApplicationService;
        public ServicoController(IServicoApplicationService servicoApplicationService)
        {
            _servicoApplicationService = servicoApplicationService;
        }
        [HttpGet]
        [AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var retorno = await _servicoApplicationService.BuscarServicos(Chave ?? string.Empty);
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
