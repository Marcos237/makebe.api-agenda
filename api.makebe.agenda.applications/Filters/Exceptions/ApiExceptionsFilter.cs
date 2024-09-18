using api.makebe.agenda.applications.Exceptions;
using api.makebe.agenda.applications.Models.Responses;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace api.makebe.session.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode statusCode = context.Exception switch
            {
                BadRequestException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Message = context.Exception.Message
            };
            context.Result = new JsonResult(responseModel) { StatusCode = (int)statusCode };
        }
    }
}
