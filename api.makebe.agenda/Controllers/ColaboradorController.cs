using api.makebe.agenda.applications.Interfaces;
using api.makebe.agenda.applications.Models.Payloads;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.agenda.Controllers
{
    [ApiController]
    [Route("Colaborador")]
    //[Authorize]
    public class ColaboradorController : BaseController
    {
        private readonly IColaboradorApplicationService _colaboradorApplicationService;
        public ColaboradorController(IColaboradorApplicationService colaboradorApplicationService)
        {
            _colaboradorApplicationService = colaboradorApplicationService;
        }

        [HttpPost]
        [Route("BuscarPaginado")]
        //[AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> BuscarColaboradores(ColaboradorPayload model)
        {
            try
            {
                var retorno = await _colaboradorApplicationService.BuscarUsuario(model, Chave ?? string.Empty);
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
        [Route("PersistirColaborador")]
        //[AuthorizationFilter(PapeisPermissoes.GerenciaContasGestor)]
        public async Task<IActionResult> PersistirColaborador(ColaboradorPayload model)
        {
            try
            {
                var retorno = await _colaboradorApplicationService.SalvarUsuario(model, Chave ?? string.Empty);
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

    }
}
