using BLL.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokeAPI.Helpers;
using System;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using BLL.Extensions.Logging;

namespace PokeAPI.MiddleWare
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //Check if there was an exeption. If not continue the pipeline
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogLevel logLevel;

                // Write correct http response error
                var response = context.Response;

                if (ex is NullReferenceException)
                {
                    logLevel = LogLevel.Error;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                else if (ex is UnauthorizedAccessException)
                {
                    logLevel = LogLevel.Error;
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                }
                else if (ex is UserActionException)
                {
                    // This are errors caused by the user (wrong parameters)
                    // Return a 'message' so we can inform the user what he/she did wrong
                    logLevel = LogLevel.Warning;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.ContentType = "application/json";
                    await response.WriteAsync(JsonConvert.SerializeObject(((UserActionException) ex).Errors));
                }
                else if (ex is AuthenticationException)
                {
                    // This are errors caused by the user (wrong login)
                    // But we don't want to show the reseaon to minimalize 'User Enumeration'
                    logLevel = LogLevel.Warning;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    // Other exeptions are caused by wrong code
                    // Example ArgumentNullExecption => Some business code needed a certain argument but it was null
                    // This is caused by unsufficient checking in the controller. 
                    // Otherwise the ModelState should already have catched this issue.
                    logLevel = LogLevel.Critical;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }

                // Log exeption
                _logger.LogException(logLevel, ex);
            }
        }
    }
}
