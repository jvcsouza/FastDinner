namespace FastDinner.Contracts.Menu;

public record CreateMenuRequest(
    string Name,
    string Description,
    byte[] Image);