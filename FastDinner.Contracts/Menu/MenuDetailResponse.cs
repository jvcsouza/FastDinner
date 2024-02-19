namespace FastDinner.Contracts.Menu;

public record MenuDetailResponse(
    Guid Id,
    string Name,
    string Description,
    string Image,
    bool Main,
    IEnumerable<CategoryMenuResponse> Categories
);

public record CategoryMenuResponse(
    Guid Id,
    string Name,
    string Description,
    IEnumerable<MenuItemResponse> MenuItems
);

public record MenuItemResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price
);