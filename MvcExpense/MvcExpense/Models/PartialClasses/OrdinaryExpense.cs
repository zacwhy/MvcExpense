using System;

namespace MvcExpense.Models
{
    public partial class OrdinaryExpense : ICloneable
    {
        public object Clone()
        {
            var ordinaryExpense = (OrdinaryExpense) this.MemberwiseClone();
            return ordinaryExpense;
        }

        public OrdinaryExpense CloneOrdinaryExpense()
        {
            return (OrdinaryExpense) Clone();
        }

        public override string ToString()
        {
            string s = string.Format( "{0} ({1}) {2} {3}", Date, Sequence, Price, Description );
            return s;
        }

    }
}