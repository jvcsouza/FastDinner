using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDinner.Domain.Contracts
{
    public interface IRestaurant
    {
        Guid RestaurantId { get; set; }
    }
}
