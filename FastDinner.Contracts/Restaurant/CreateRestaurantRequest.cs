using FastDinner.Contracts.Restaurant;

namespace FastDinner.Contracts.Menu;

public record CreateRestaurantRequest (
    string Name,
    string Address,
    string Phone,
    string Email
);