namespace FastDinner.Contracts.Menu;

public record AddCategoryMenuRequest(
    Guid MenuId,
    string Name,
    string Description);