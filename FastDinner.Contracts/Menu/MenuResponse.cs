namespace FastDinner.Contracts.Menu;

public record MenuResponse(
    Guid Id,
    string Name,
    string Description,
    string Image
);