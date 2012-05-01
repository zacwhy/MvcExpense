using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcExpense.Models;

namespace MvcExpense.ViewModels
{
    public class OrdinaryExpenseEditModel
    {
        public long Id { get; set; }

        [DataType( DataType.Date )]
        public DateTime Date { get; set; }

        public int Sequence { get; set; }

        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        public long CategoryId { get; set; }

        public long ConsumerId { get; set; }

        public long? PaymentMethodId { get; set; }

        public string Remarks { get; set; }

        //
        // Display
        //

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

        public SelectList ConsumersSelectList
        {
            get
            {
                var selectList = new SelectList( Consumers, "Id", "Name" );
                return selectList;
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

    }
}