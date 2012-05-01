using System.Linq;
using System.Web.Mvc;
using MvcExpense.Models;

namespace MvcExpense.ViewModels
{
    public class OrdinaryExpenseCreateModel : OrdinaryExpenseEditModelBase
    {
        public long[] SelectedConsumerIds { get; set; }

        public MultiSelectList ConsumersMultiSelectList
        {
            get
            {
                IOrderedEnumerable<Consumer> orderedItems = Consumers.OrderBy( x => x.Sequence );
                var multiSelectList = new MultiSelectList( orderedItems, "Id", "Name", SelectedConsumerIds );
                return multiSelectList;
            }
        }

    }
}