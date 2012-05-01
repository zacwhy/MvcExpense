using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MvcExpense.Models;

namespace MvcExpense.ViewModels
{
    public class OrdinaryExpenseEditModelBase
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
                return OrdinaryExpenseModelHelper.GetCategoriesGroupSelectList( Categories );
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

    }
}