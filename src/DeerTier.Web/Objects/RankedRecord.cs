namespace DeerTier.Web.Objects
{
    public class RankedRecord
    {
        public RankedRecord(Record record)
        {
            Record = record;
        }

        public Record Record { get; }
        public int Rank { get; set; }
    }
}