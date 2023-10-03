using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Domain.Model;

namespace FastDinner.Infrastructure.Persistence.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(DinnerContext context) : base(context)
        {
        }
    }
}
