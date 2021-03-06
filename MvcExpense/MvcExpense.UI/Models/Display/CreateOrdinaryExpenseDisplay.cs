﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcExpense.Core.Models;
using MvcExpense.Services;
using MvcExpense.UI.Models.Input;

namespace MvcExpense.UI.Models.Display
{
    public class CreateOrdinaryExpenseDisplay : CreateOrdinaryExpenseInput
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Consumer> Consumers { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }

        public IEnumerable<SelectListGroupItem> CategoriesGroupSelectList
        {
            get
            {
                return OrdinaryExpenseService.GetCategoriesGroupSelectList( Categories );
            }
        }

        public SelectList PaymentMethodsSelectList
        {
            get
            {
                IOrderedEnumerable<PaymentMethod> orderedItems = PaymentMethods.OrderBy( x => x.Sequence );
                return new SelectList( orderedItems, "Id", "Name" );
            }
        }

        public MultiSelectList ConsumersMultiSelectList
        {
            get
            {
                IOrderedEnumerable<Consumer> orderedItems = Consumers.OrderBy( x => x.Sequence );
                return new MultiSelectList( orderedItems, "Id", "Name", SelectedConsumerIds );
            }
        }

    }
}