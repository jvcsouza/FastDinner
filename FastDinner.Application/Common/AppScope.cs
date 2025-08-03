namespace FastDinner.Application.Common
{
    public class AppScope
    {
        public static TenantSettings Tenant => RequestContext<TenantSettings>.GetData("tenant_key");
        public static RestaurantSettings Restaurant => RequestContext<RestaurantSettings>.GetData("restaurant_key");

        public Guid? RestaurantId = Restaurant?.RestaurantId;

    }
}
