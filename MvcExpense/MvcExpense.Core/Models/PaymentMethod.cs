using System.Collections.Generic;
using Zac.DesignPattern.Models;

namespace MvcExpense.Core.Models
{
    public class PaymentMethod : StandardPersistentObject
    {
        public PaymentMethod()
        {
            OrdinaryExpenses = new HashSet<OrdinaryExpense>();
        }
    
        public string Name { get; set; }
        public int Sequence { get; set; }
    
        public virtual ICollection<OrdinaryExpense> OrdinaryExpenses { get; set; }
    }
}
