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
    
    public partial class Category
    {
        public Category()
        {
            this.Children = new HashSet<Category>();
            this.OrdinaryExpenses = new HashSet<OrdinaryExpense>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
        public Nullable<long> ParentId { get; set; }
        public long lft { get; set; }
        public long rgt { get; set; }
    
        public virtual ICollection<Category> Children { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<OrdinaryExpense> OrdinaryExpenses { get; set; }
    }
}
