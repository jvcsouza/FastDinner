using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDinner.Contracts.Table;

public record CreateTableRequest(
    string Description,
    int Seats
);