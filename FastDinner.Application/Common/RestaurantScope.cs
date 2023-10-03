﻿namespace FastDinner.Application.Common
{
    public static class RestaurantScope
    {
        private const string TenantKey = "restaurant_key";

        public static void CreateScope(RestaurantSettings context)
        {
            CallContext<RestaurantSettings>.SetData(TenantKey, context);
        }

        public static RestaurantSettings GetTenantScope()
        {
            return CallContext<RestaurantSettings>.GetData(TenantKey);
        }
    }


    public class RestaurantSettings
    {
        public Guid ResturantId { get; set; }
        public string Name { get; set; }
    }
}
