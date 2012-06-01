using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MvcExpense.Models;

namespace MvcExpense.ViewModels
{
    public class OrdinaryExpenseCreateEditModelBase
    {
        public long Id { get; set; }

        [DataType( DataType.Date )]
        public DateTime Date { get; set; }

        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        public long CategoryId { get; set; }

        public long? PaymentMethodId { get; set; }

        public string Remarks { get; set; }

        public IList<Category> Categories { get; set; }
        public IList<PaymentMethod> PaymentMethods { get; set; }
        public IList<Consumer> Consumers { get; set; }

        public IList<SelectListGroupItem> CategoriesGroupSelectList
        {
            get
            {
                return GetCategoriesGroupSelectList( Categories );
            }
        }

        public SelectList PaymentMethodsSelectList
        {
            get
            {
                IOrderedEnumerable<PaymentMethod> orderedItems = PaymentMethods.OrderBy( x => x.Sequence );
                var selectList = new SelectList( orderedItems, "Id", "Name" );
                return selectList;
            }
        }

        private static IList<SelectListGroupItem> GetCategoriesGroupSelectList( IList<Category> categories )
        {
            IEnumerable<IGrouping<string, Category>> queryLeaves =
                    from x in categories
                    where x.Children.Count == 0
                    group x by x.Parent.Name into grouping
                    select grouping;

            var list = new List<SelectListGroupItem>();

            foreach ( IGrouping<string, Category> group in queryLeaves )
            {
                var selectListGroupItem = new SelectListGroupItem { Name = group.Key };
                selectListGroupItem.Items = new List<SelectListItem>();

                foreach ( Category category in group )
                {
                    var selectListItem = new SelectListItem();
                    selectListItem.Text = category.Name;
                    selectListItem.Value = category.Id.ToString();
                    selectListGroupItem.Items.Add( selectListItem );
                }

                list.Add( selectListGroupItem );
            }

            return list;
        }

    }
}