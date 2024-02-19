namespace FastDinner.Contracts.Menu;

public record AddItemToCategoryMenuRequest(
    Guid MenuId,
    Guid CategoryId,
    Guid ProductId,
    string ProductDescription,
    decimal Price
);