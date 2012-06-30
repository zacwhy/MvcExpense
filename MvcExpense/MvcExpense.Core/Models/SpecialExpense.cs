using System;
using Zac.DesignPattern.Models;

namespace MvcExpense.Core.Models
{
    public class SpecialExpense : StandardPersistentObject
    {
        public DateTime Date { get; set; }
        public int Sequence { get; set; }
        public double Price { get; set; }
        public long ConsumerId { get; set; }
        public string Description { get; set; }
    
        public virtual Consumer Consumer { get; set; }
    }
}
