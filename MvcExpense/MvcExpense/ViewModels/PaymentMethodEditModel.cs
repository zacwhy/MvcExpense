using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcExpense.Models;

namespace MvcExpense.ViewModels
{
    public class PaymentMethodEditModel
    {
        private static readonly SelectListItem BeginningSelectListItem = new SelectListItem
        {
            Text = "(beginning)",
            Value = ( 0 ).ToString()
        };

        public long Id { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }

        public int PreviousItemSequence { get; set; }

        public IList<PaymentMethod> PaymentMethods { get; set; }

        public SelectList SequenceSelectList
        {
            get
            {
                IEnumerable<SelectListItem> selectListItems =
                    from x in PaymentMethods
                    where x.Id != Id
                    orderby x.Sequence
                    select new SelectListItem
                    {
                        Value = x.Sequence.ToString(),
                        Text = x.Sequence + ". " + x.Name
                    };

                List<SelectListItem> list = selectListItems.ToList();
                list.Insert( 0, BeginningSelectListItem );

                var selectList = new SelectList( list, "Value", "Text" );
                return selectList;
            }
        }

    }
}