using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        //public long ConsumerId { get; set; }

        [Required]
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
                //foreach ( Category category in Categories )
                //{
                //    Category parent = category.Parent;
                //}

                IEnumerable<IGrouping<string, Category>> leavesQuery =
                    from x in Categories
                    where x.Children.Count == 0
                    group x by x.Parent.Name into grouping
                    select grouping;

                var list = new List<SelectListGroupItem>();

                foreach ( IGrouping<string, Category> group in leavesQuery )
                {
                    var selectListGroupItem = new SelectListGroupItem { Name = group.Key };
                    selectListGroupItem.Items= new List<SelectListItem>();

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