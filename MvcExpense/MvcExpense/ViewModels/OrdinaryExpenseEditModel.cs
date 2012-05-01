using System.Linq;
using System.Web.Mvc;
using MvcExpense.Models;

namespace MvcExpense.ViewModels
{
    public class OrdinaryExpenseEditModel : OrdinaryExpenseEditModelBase
    {
        public int Sequence { get; set; }

        public long ConsumerId { get; set; }

        public SelectList ConsumersSelectList
        {
            get
            {
                IOrderedEnumerable<Consumer> orderedItems = Consumers.OrderBy( x => x.Sequence );
                var selectList = new SelectList( Consumers, "Id", "Name" );
                return selectList;
            }
        }

    }
}