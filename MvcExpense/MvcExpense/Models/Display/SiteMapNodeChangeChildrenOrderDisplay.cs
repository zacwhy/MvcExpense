using System.Collections.Generic;
using System.Web.Mvc;
using MvcExpense.UI.Models.Input;
using Zac.StandardCore.Models;

namespace MvcExpense.UI.Models.Display
{
    public class SiteMapNodeChangeChildrenOrderDisplay : SiteMapNodeChangeChildrenOrderInput
    {
        public IEnumerable<SiteMapNode> Children { get; set; }

        public SelectList ChildrenSelectList
        {
            get
            {
                return new SelectList( Children, "Id", "Title" );
            }
        }

    }
}