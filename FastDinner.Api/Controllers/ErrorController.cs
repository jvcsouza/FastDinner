using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastDinner.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    // ReSharper disable once UnusedMember.Global
    protected IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        var (statusCode, message) = exception switch
        {
            ApplicationException applicationException => (400, applicationException.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        return Problem(statusCode: statusCode, title: message);
    }
}