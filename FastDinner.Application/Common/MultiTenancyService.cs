using FastDinner.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace FastDinner.Application.Common
{
    public class MultiTenancyService : IMultiTenancyService
    {
        private readonly IHttpContextAccessor _httpContext;

        public MultiTenancyService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }


        public TenantSettings Tenant => _httpContext.HttpContext.Items["tenant_key"] as TenantSettings;
        public RestaurantSettings Restaurant => _httpContext.HttpContext.Items["restaurant_key"] as RestaurantSettings;
    }
}
