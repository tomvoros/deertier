namespace DeerTier.Web.Objects
{
    public class FormattedTime
    {
        public FormattedTime(int timeSeconds, string timeString)
        {
            TimeSeconds = timeSeconds;
            TimeString = timeString;
        }

        public int TimeSeconds { get; private set; }
        public string TimeString { get; private set; }
    }
}