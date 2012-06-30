
namespace MvcExpense.HelperModels
{
    public class CategoryBreakdownRecord
    {
        public string Label { get; set; }

        public double Breakfast { get; set; }
        public double Lunch { get; set; }
        public double Dinner { get; set; }
        public double OtherFood { get; set; }

        public double Bus { get; set; }
        public double Train { get; set; }
        public double Taxi { get; set; }

        public double Petrol { get; set; }
        public double Parking { get; set; }
        public double Erp { get; set; }

        public double Food
        {
            get
            {
                return Breakfast + Lunch + Dinner + OtherFood;
            }
        }

        public double PublicTransport
        {
            get
            {
                return Bus + Train + Taxi;
            }
        }

        public double Car
        {
            get
            {
                return Petrol + Parking + Erp;
            }
        }

        public double Transport
        {
            get
            {
                return PublicTransport + Car;
            }
        }

        public double All
        {
            get
            {
                return Food + Transport;
            }
        }

        public CategoryBreakdownRecord( string label )
        {
            Label = label;
        }

    }
}