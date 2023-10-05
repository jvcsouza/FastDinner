namespace FastDinner.Application.Common
{
    public class AppScope
    {
        public static TenantSettings Tenant => CallContext<TenantSettings>.GetData("tenant_key");
        public static RestaurantSettings Restaurant => CallContext<RestaurantSettings>.GetData("restaurant_key");
        
        public Guid? RestaurantId = Restaurant?.ResturantId;

    }
}
