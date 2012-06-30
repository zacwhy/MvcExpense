using System.Collections.Generic;
using Zac.DesignPattern.Models;
    
namespace MvcExpense.Core.Models
{
    public class Category : StandardPersistentObject
    {
        public Category()
        {
            Children = new HashSet<Category>();
            OrdinaryExpenses = new HashSet<OrdinaryExpense>();
        }

        public string Name { get; set; }
        public long? ParentId { get; set; }
        public long lft { get; set; }
        public long rgt { get; set; }

        public virtual ICollection<Category> Children { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<OrdinaryExpense> OrdinaryExpenses { get; set; }
    }
}
