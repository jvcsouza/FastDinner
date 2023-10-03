namespace FastDinner.Contracts.Restaurant;

public record UpdateRestaurantRequest (
    Guid Id,
    string Name,
    string Address,
    string Phone,
    string Email
);