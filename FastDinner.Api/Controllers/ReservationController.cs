using MediatR;

namespace FastDinner.Api.Controllers
{
    public class ReservationController : ApiController
    {
        private readonly ISender _mediator;

        public ReservationController(ISender mediator)
        {
            _mediator = mediator;
        }
    }
}
