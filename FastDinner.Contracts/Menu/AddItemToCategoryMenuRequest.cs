namespace FastDinner.Contracts.Menu;

public record AddItemToCategoryMenuRequest(
    Guid MenuId,
    Guid CategoryId,
    Guid ProductId,
    string ProductName,
    string ProductDescription,
    decimal Price
);