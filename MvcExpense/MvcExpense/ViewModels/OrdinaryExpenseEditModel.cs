using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Web.Mvc;
using System.Collections.Generic;
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

        public string Remarks { get; set; }

        public long ConsumerId { get; set; }

        //[Required]
        public long CategoryId { get; set; }

        public long? PaymentMethodId { get; set; }

        public IList<Category> Categories { get; set; }
        public IList<PaymentMethod> PaymentMethods { get; set; }
        public IList<Consumer> Consumers { get; set; }

        public long[] SelectedConsumerIds { get; set; }

        public SelectList CategoriesSelectList
        {
            get
            {
                var selectList = new SelectList( Categories, "Id", "Name"/*, SelectedCategoryId*/ );
                return selectList;
            }
        }

        public IEnumerable<GroupedSelectListItem> CategoriesGroupedSelectList
        {
            get
            {
                yield return new GroupedSelectListItem { GroupKey = "1", GroupName = "Food", Text = "Breakfast", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "1", GroupName = null, Text = "Lunch", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "1", GroupName = null, Text = "Dinner", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "1", GroupName = null, Text = "Other Meal", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "2", GroupName = "Public Transport", Text = "Bus (Work)", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "2", GroupName = "Transport2", Text = "Train (Work)", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "2", GroupName = "Public Transport", Text = "Bus", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "2", GroupName = "Transport2", Text = "Train", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "2", GroupName = "Transport", Text = "Taxi", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "3", GroupName = "Car", Text = "Petrol", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "3", GroupName = "Car", Text = "Parking", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "3", GroupName = "Car", Text = "Road", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "4", GroupName = "All", Text = "Treat", Value = "a1" };
                yield return new GroupedSelectListItem { GroupKey = "4", GroupName = "All", Text = "Other", Value = "a1" };
            }
        }

        public SelectList PaymentMethodsSelectList
        {
            get
            {
                var selectList = new SelectList( PaymentMethods, "Id", "Name"/*, SelectedCategoryId*/ );
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