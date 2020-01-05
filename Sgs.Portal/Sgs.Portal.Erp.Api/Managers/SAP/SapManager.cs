using SAP.Middleware.Connector;

namespace Sgs.Portal.Erp.Api.Managers.SAP
{
    public class SapManager 
    {
        private readonly IDestinationConfiguration _destinationConfiguration;
        private readonly ISapConfiguration _sapConfiguration;

        protected readonly RfcDestination rfcDestination;
        protected readonly RfcRepository rfcRepository;

        public SapManager(IDestinationConfiguration destinationConfiguration, 
            ISapConfiguration sapConfiguration)
        {
            _destinationConfiguration = destinationConfiguration;
            _sapConfiguration = sapConfiguration;

            if (RfcDestinationManager.TryGetDestination(destinationName: _sapConfiguration.DestinationName) == null)
            {
                RfcDestinationManager.RegisterDestinationConfiguration(_destinationConfiguration);
            }

        rfcDestination = RfcDestinationManager
                .GetDestination(destinationName: _sapConfiguration.DestinationName);
            rfcRepository = rfcDestination.Repository;
        }

        protected IRfcFunction getIRfcFunction(string functionName)
        {
            return rfcRepository.CreateFunction(functionName);
        }
    }
}