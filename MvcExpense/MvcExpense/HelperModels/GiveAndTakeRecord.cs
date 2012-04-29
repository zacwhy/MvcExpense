
namespace MvcExpense.HelperModels
{
    public class GiveAndTakeRecord
    {
        public string Party { get; set; }
        public double GiveAmount { get; set; }
        public double TakeAmount { get; set; }

        public double OweAmount
        {
            get
            {
                return TakeAmount - GiveAmount;
            }
        }

        public GiveAndTakeRecord( string party )
        {
            Party = party;
        }

    }
}