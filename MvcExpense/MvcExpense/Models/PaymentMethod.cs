//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcExpense.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            this.OrdinaryExpenses = new HashSet<OrdinaryExpense>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<OrdinaryExpense> OrdinaryExpenses { get; set; }
    }
}
