using Microsoft.Extensions.Options;
using MultiTenancy.ViewModel;

namespace MultiTenancy.Service
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettingsViewModel _tenantSettings;
        private HttpContext _httpContext;
        private Tenant _currentTenant;
        public TenantService(IOptions<TenantSettingsViewModel> tenantSettings, IHttpContextAccessor contextAccessor)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContext = contextAccessor.HttpContext;
            if (_httpContext != null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetTenant(tenantId);

                }
                else
                {
                    throw new Exception("Invalid Tenant!");
                }
            }
        }
        private void SetTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.Where(a => a.TID == tenantId).FirstOrDefault();
            if (_currentTenant == null) throw new Exception("Invalid Tenant!");
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                SetDefaultConnectionStringToCurrentTenant();
            }
        }
        private void SetDefaultConnectionStringToCurrentTenant()
        {
            _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
        }
        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }
        public string GetDatabaseProvider()
        {
            return _tenantSettings.Defaults?.DBProvider;
        }
        public Tenant GetTenant()
        {
            return _currentTenant;
        }
    }
}
