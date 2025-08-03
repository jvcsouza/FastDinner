using System.Threading.Tasks;
using FastDinner.Application.Common.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastDinner.Api.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ISender _mediator;

        public ApiController(ISender mediator)
        {
            _mediator = mediator;
        }

        protected async Task<T> SendCommandAsync<T>(IRequest<T> command)
        {
            return await _mediator.Send(command);
        }

        protected async Task<T> SendQueryAsync<T>(IRequest<T> query)
        {
            return await _mediator.Send(query);
        }
    }
}