using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Sgs.Portal.Erp.Api.Managers.SAP
{
    public class SapConfiguration
    {
        private readonly string _configurationType;

        public string DestinationName { get; set; }
        public string AppServerHost { get; set; }
        public string SystemNumber { get; set; }
        public string SystemID { get; set; }
        public string SapUser { get; set; }
        public string Password { get; set; }
        public string Client { get; set; }
        public string Language { get; set; }
        public string PoolSize { get; set; }

        public SapConfiguration(string configurationType)
        {
            _configurationType = configurationType;
            DestinationName = ConfigurationManager.AppSettings[$"{nameof(DestinationName)}.{_configurationType}"];
            AppServerHost = ConfigurationManager.AppSettings[$"{nameof(AppServerHost)}.{_configurationType}"];
            SystemNumber = ConfigurationManager.AppSettings[$"{nameof(SystemNumber)}.{_configurationType}"];
            SystemID = ConfigurationManager.AppSettings[$"{nameof(SystemID)}.{_configurationType}"];
            SapUser = ConfigurationManager.AppSettings[$"{nameof(SapUser)}.{_configurationType}"];
            Password = ConfigurationManager.AppSettings[$"{nameof(Password)}.{_configurationType}"];
            Client = ConfigurationManager.AppSettings[$"{nameof(Client)}.{_configurationType}"];
            Language = ConfigurationManager.AppSettings[$"{nameof(Language)}.{_configurationType}"];
            PoolSize = ConfigurationManager.AppSettings[$"{nameof(PoolSize)}.{_configurationType}"];
        }
        
    }
}