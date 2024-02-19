using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Infrastructure.Persistence;
using MediatR;

namespace FastDinner.Api.Controllers
{
    public class ReservationController : ApiController
    {
        public ReservationController(ISender mediator, IUnitOfWork unitOfWork) 
            : base(mediator, unitOfWork) { }
    }
}
