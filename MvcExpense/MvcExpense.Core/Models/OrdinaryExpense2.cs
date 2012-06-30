using System;

namespace MvcExpense.Core.Models
{
    public partial class OrdinaryExpense : ICloneable
    {
        public object Clone()
        {
            return MemberwiseClone();
        }

        public OrdinaryExpense CloneOrdinaryExpense()
        {
            return (OrdinaryExpense) Clone();
        }

        public override string ToString()
        {
            return string.Format( "{0} ({1}) {2} {3}", Date, Sequence, Price, Description );
        }

    }
}