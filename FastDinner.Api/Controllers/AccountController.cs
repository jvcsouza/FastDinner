using System.Threading.Tasks;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastDinner.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ISender _mediator;

    public AccountController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _mediator.Send(new LoginQuery(request.Username, request.Password));

        return Ok(response);
    }

    // public async Task<IActionResult> Logout()
    // {
    //     return Ok();
    // }
}