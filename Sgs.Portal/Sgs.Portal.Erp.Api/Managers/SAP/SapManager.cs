using SAP.Middleware.Connector;
using System;

namespace Sgs.Portal.Erp.Api.Managers.SAP
{
    public class SapManager 
    {
        private readonly IDestinationConfiguration _destinationConfiguration;
        private readonly ISapConfiguration _sapConfiguration;

        protected RfcDestination rfcDestination;
        protected RfcRepository rfcRepository;

        public SapManager(IDestinationConfiguration destinationConfiguration, 
            ISapConfiguration sapConfiguration)
        {
            try
            {
                _destinationConfiguration = destinationConfiguration;
                _sapConfiguration = sapConfiguration;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected IRfcFunction getIRfcFunction(string functionName)
        {
            try
            {
                if (RfcDestinationManager.TryGetDestination(destinationName: _sapConfiguration.DestinationName) == null)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(_destinationConfiguration);
                }

                if (rfcDestination == null || rfcRepository == null)
                {
                    rfcDestination = RfcDestinationManager
                   .GetDestination(destinationName: _sapConfiguration.DestinationName);
                    rfcRepository = rfcDestination.Repository;
                }

                return rfcRepository.CreateFunction(functionName);
            }
            catch (Exception)
            {
                throw new Exception("Check Sap Connection !");
            }
        }
    }
}