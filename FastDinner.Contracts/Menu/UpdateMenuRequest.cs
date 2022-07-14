namespace FastDinner.Contracts.Menu;

public record UpdateMenuRequest(
    Guid Id,
    string Name,
    string Description,
    byte[] Image);