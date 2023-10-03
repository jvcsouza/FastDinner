using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDinner.Domain.Model
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
    }
}
