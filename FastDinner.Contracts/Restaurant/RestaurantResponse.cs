namespace FastDinner.Contracts.Restaurant;

public record RestaurantResponse (
    Guid Id,
    string Name,
    string Address,
    string Phone,
    string Email
);