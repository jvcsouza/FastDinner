namespace FastDinner.Contracts.Restaurant;

public record CreateRestaurantRequest (
    string Name,
    string Address,
    string Phone,
    string Email
);