using System.Configuration;

namespace Sgs.Portal.Erp.Api.Managers.SAP
{
    public class SapConfiguration : ISapConfiguration
    {
        private readonly string _configurationType;

        public SapConfiguration(string configurationType)
        {
            _configurationType = configurationType;
        }

        public string DestinationName => 
            ConfigurationManager.AppSettings[$"{nameof(DestinationName)}.{_configurationType}"];

        public string AppServerHost => 
            ConfigurationManager.AppSettings[$"{nameof(AppServerHost)}.{_configurationType}"];

        public string SystemNumber => 
            ConfigurationManager.AppSettings[$"{nameof(SystemNumber)}.{_configurationType}"];

        public string SystemID => 
            ConfigurationManager.AppSettings[$"{nameof(SystemID)}.{_configurationType}"];

        public string SapUser =>
            ConfigurationManager.AppSettings[$"{nameof(SapUser)}.{_configurationType}"];

        public string SapPassword =>
            ConfigurationManager.AppSettings[$"{nameof(SapPassword)}.{_configurationType}"];

        public string SapClient =>
            ConfigurationManager.AppSettings[$"{nameof(SapClient)}.{_configurationType}"];

        public string Language =>
            ConfigurationManager.AppSettings[$"{nameof(Language)}.{_configurationType}"];

        public string PoolSize =>
            ConfigurationManager.AppSettings[$"{nameof(PoolSize)}.{_configurationType}"];
    }
}