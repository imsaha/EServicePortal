using EServicePortal.Application.Common.Wrappers;
using EServicePortal.Application.Users.Commands;
using EServicePortal.Application.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EServicePortal.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route(nameof(Signup))]
    [Produces(typeof(Response))]
    public async Task<IActionResult> Signup([FromBody]SignupCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPost]
    [Route(nameof(Login))]
    [Produces(typeof(Response<LoginResponse>))]
    public async Task<IActionResult> Login([FromBody]LoginCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}
