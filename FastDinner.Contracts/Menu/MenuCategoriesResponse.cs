namespace FastDinner.Contracts.Menu;

public record MenuCategoriesResponse(
    Guid Id,
    string Name,
    string Description,
    int QtyProducts
);