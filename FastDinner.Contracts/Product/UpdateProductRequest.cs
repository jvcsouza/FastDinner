namespace FastDinner.Contracts.Product;

public record UpdateProductRequest (
    Guid Id,
    string Name
);