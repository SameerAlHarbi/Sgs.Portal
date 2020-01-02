namespace Sgs.Portal.Shared.Helpers
{
    public class LoggingEvents
    {
        public const int ProcessStarted = 1000;

        public const int Step1KickedOff = 1001;
        public const int Step1InProcess = 1002;
        public const int Step1Completed = 1003;

        public const int GettingDataStarted = 1004;
        public const int GettingDataError = 1005;

        public const int InsertDataStarted = 1010;
        public const int InsertDataFinished = 1015;
        public const int InsertDataValidationError = 1019;
        public const int InsertDataError = 1020;
    }
}
