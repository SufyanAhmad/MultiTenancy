namespace MultiTenancy.ViewModel
{
    public class TenantSettingsViewModel
    {
        public Configuration Defaults { get; set; }
        public List<Tenant> Tenants { get; set; }
    }
    public class Tenant
    {
        public string Name { get; set; }
        public string TID { get; set; }
        public string ConnectionString { get; set; }
    }
    public class Configuration
    {
        public string DBProvider { get; set; }
        public string ConnectionString { get; set; }
    }
}
