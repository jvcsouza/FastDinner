namespace FastDinner.Application.Common
{
    public class AppScope
    {
        private static TenantSettings _tenant;
        private static RestaurantSettings _restaurant;

        public static TenantSettings Tenant => _tenant;
        public static RestaurantSettings Restaurant => _restaurant;

        public void UseTenant(TenantSettings tenant) => _tenant = tenant;
        public void UseRestaurant(RestaurantSettings restaurant) => _restaurant = restaurant;

        public void DefineScope(TenantSettings tenant, RestaurantSettings restaurant)
        {
            _tenant = tenant;
            _restaurant = restaurant;
        }
    }
}
