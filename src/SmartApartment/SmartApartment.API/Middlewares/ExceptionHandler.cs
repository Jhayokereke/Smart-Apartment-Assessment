using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SmartApartment.Application.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartApartment.API.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        public ExceptionHandler(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ConvertException(context, ex);
            }

        }

        private Task ConvertException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse();

            HttpStatusCode httpStatusCode;
            switch (ex)
            {
                case ValidationException validationException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    response.ValidationErrors = validationException.ValidationErrors;
                    break;
                default:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.StatusCode = (int)httpStatusCode;

            if (httpStatusCode == HttpStatusCode.InternalServerError)
            {
                response.Message = _env.IsDevelopment() ? ex.Message : "INTERNAL SERVER ERROR";
            }
            else
            {
                response.Message = ex.Message;
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
