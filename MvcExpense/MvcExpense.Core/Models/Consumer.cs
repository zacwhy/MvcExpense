using System.Collections.Generic;
using Zac.DesignPattern.Models;

namespace MvcExpense.Core.Models
{
    public partial class Consumer : StandardPersistentObject
    {
        public Consumer()
        {
            SpecialExpenses = new HashSet<SpecialExpense>();
            OrdinaryExpenses = new HashSet<OrdinaryExpense>();
        }
    
        public string Name { get; set; }
        public int Sequence { get; set; }
    
        public virtual ICollection<SpecialExpense> SpecialExpenses { get; set; }
        public virtual ICollection<OrdinaryExpense> OrdinaryExpenses { get; set; }
    }
}
