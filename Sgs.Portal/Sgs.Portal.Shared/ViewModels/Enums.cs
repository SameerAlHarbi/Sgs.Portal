namespace Sgs.Portal.Shared
{
    public enum RequestStatus
    {
        Initial,
        New,
        Validating,
        Processing,
        Accepted,
        Canceled,
        Expired,
        Rejected
    }

    public enum ActionType
    {
        Create,
        Confirm,
        Update,
        Delete,
        Cancel,
        Expire,
        Accept,
        Reject
    }
}