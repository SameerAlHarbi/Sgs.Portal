using SAP.Middleware.Connector;

namespace Sgs.Portal.Erp.Api.Managers.SAP
{
    public class SapDestinationConfiguration : IDestinationConfiguration
    {
        private readonly ISapConfiguration _sapConfiguration;

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public bool ChangeEventsSupported()
        {
            return false;
        }

        public SapDestinationConfiguration(ISapConfiguration sapConfiguration)
        {
            _sapConfiguration = sapConfiguration;
        }

        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters parms = new RfcConfigParameters();

            if (destinationName.Equals(destinationName))
            {
                parms.Add(RfcConfigParameters.AppServerHost, _sapConfiguration.AppServerHost);
                parms.Add(RfcConfigParameters.SystemNumber, _sapConfiguration.SystemNumber);
                parms.Add(RfcConfigParameters.SystemID, _sapConfiguration.SystemID);
                parms.Add(RfcConfigParameters.User, _sapConfiguration.SapUser);
                parms.Add(RfcConfigParameters.Password, _sapConfiguration.SapPassword);
                parms.Add(RfcConfigParameters.Client, _sapConfiguration.SapClient);
                parms.Add(RfcConfigParameters.Language, _sapConfiguration.Language);
                parms.Add(RfcConfigParameters.PoolSize, _sapConfiguration.PoolSize);
            }

            return parms;
        }
    }
}