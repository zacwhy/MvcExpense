using System.Web.Mvc;
using MvcExpense.UI.Extensions;
using MvcExpense.UI.Models.Input;
using Zac.StandardCore.Models;
using Zac.Tree;

namespace MvcExpense.UI.Models.Display
{
    public class SiteMapNodeCreateDisplay : SiteMapNodeCreateInput
    {
        public TreeNode<SiteMapNode> Tree { get; set; }

        public SelectList ParentSelectList
        {
            get
            {
                return Tree.ToSelectList();
            }
        }

        public MvcHtmlString TreeJson
        {
            get { return new MvcHtmlString( Tree.ToJson() ); }
        }

    }
}