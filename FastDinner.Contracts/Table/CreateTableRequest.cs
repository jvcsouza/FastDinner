namespace FastDinner.Contracts.Table;

public record CreateTableRequest(
    string Description,
    int Seats
);