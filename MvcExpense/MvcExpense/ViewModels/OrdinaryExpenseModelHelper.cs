using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcExpense.Models;

namespace MvcExpense.ViewModels
{
    public static class OrdinaryExpenseModelHelper
    {
        public static IList<SelectListGroupItem> GetCategoriesGroupSelectList( IList<Category> categories )
        {
            IEnumerable<IGrouping<string, Category>> leavesQuery =
                    from x in categories
                    where x.Children.Count == 0
                    group x by x.Parent.Name into grouping
                    select grouping;

            var list = new List<SelectListGroupItem>();

            foreach ( IGrouping<string, Category> group in leavesQuery )
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