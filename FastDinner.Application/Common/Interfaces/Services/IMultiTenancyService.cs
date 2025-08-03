namespace FastDinner.Application.Common.Interfaces.Services
{
    public interface IMultiTenancyService
    {
        TenantSettings Tenant { get; }
        RestaurantSettings Restaurant { get; }
    }
}
