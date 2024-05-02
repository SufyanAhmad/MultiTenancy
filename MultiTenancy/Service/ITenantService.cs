using MultiTenancy.ViewModel;

namespace MultiTenancy.Service
{
    public interface ITenantService
    {
        public string GetDatabaseProvider();
        public string GetConnectionString();
        public Tenant GetTenant();
    }
}
