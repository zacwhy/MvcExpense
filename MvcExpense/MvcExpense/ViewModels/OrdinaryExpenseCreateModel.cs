using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcExpense.Models;

namespace MvcExpense.ViewModels
{
    public class OrdinaryExpenseCreateModel
    {
        public long Id { get; set; }

        [DataType( DataType.Date )]
        public DateTime Date { get; set; }

        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        public string Remarks { get; set; }

        public long CategoryId { get; set; }

        public long? PaymentMethodId { get; set; }

        //
        // Display
        //

        public IList<Category> Categories { get; set; }
        public IList<PaymentMethod> PaymentMethods { get; set; }
        public IList<Consumer> Consumers { get; set; }

        public long[] SelectedConsumerIds { get; set; }

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
                var selectList = new SelectList( PaymentMethods, "Id", "Name" );
                return selectList;
            }
        }

        public MultiSelectList ConsumersMultiSelectList
        {
            get
            {
                var multiSelectList = new MultiSelectList( Consumers, "Id", "Name", SelectedConsumerIds );
                return multiSelectList;
            }
        }

    }
}