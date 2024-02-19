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
        private readonly IUnitOfWork _unitOfWork;

        public ApiController(ISender mediator, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        protected async Task<T> SendCommandAsync<T>(IRequest<T> command)
        {
            return await _unitOfWork.ExecuteTransaction(async () => await _mediator.Send(command));
        }

        protected async Task<T> SendQueryAsync<T>(IRequest<T> query)
        {
            return await _mediator.Send(query);
        }
    }
}