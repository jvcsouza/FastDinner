using MediatR;

namespace FastDinner.Api.Controllers
{
    public class ReservationController : ApiController
    {
        public ReservationController(ISender mediator)
            : base(mediator) { }
    }
}
