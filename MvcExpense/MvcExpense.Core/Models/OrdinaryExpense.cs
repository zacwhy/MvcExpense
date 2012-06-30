using System;
using Zac.DesignPattern.Models;

namespace MvcExpense.Core.Models
{
    public partial class OrdinaryExpense : StandardPersistentObject
    {
        public DateTime Date { get; set; }
        public int Sequence { get; set; }
        public double Price { get; set; }
        public long ConsumerId { get; set; }
        public long CategoryId { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public long? PaymentMethodId { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Consumer Consumer { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
