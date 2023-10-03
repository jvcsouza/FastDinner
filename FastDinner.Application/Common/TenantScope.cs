using System.Text.Json;

namespace FastDinner.Application.Common
{
    public static class TenantScope
    {
        private const string TenantKey = "tenant_key";

        public static void CreateScope(TenantSettings context)
        {
            CallContext<TenantSettings>.SetData(TenantKey, context);
        }

        public static TenantSettings GetTenantScope()
        {
            return CallContext<TenantSettings>.GetData(TenantKey);
        }
    }

    public class AppTenantSettings
    {
        public string Data { get; set; }
        public TenantSettings Settings => JsonSerializer.Deserialize<TenantSettings>(Data);
    }

    public class TenantSettings
    {
        public string Name { get; set; }
        public string Dns { get; set; }
        public string DataSource { get; set; }
        public string Catalog { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

}
