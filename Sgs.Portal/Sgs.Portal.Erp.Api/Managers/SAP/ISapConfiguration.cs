namespace Sgs.Portal.Erp.Api.Managers.SAP
{
    public interface ISapConfiguration
    {
        string DestinationName { get; }
        string AppServerHost { get; }
        string SystemNumber { get; }
        string SystemID { get; }
        string SapUser { get; }
        string SapPassword { get; }
        string SapClient { get; }
        string Language { get; }
        string PoolSize { get; }
    }
}
