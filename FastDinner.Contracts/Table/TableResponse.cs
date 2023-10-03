namespace FastDinner.Contracts.Table;

public record TableResponse(
    Guid Id,
    string Name,
    int Seats
);