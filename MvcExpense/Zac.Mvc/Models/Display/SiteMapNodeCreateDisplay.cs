using System.Web.Mvc;
using Zac.Mvc.Extensions;
using Zac.Mvc.Models.Input;
using Zac.StandardCore.Models;
using Zac.Tree;

namespace Zac.Mvc.Models.Display
{
    public class SiteMapNodeCreateDisplay : SiteMapNodeCreateInput
    {
        public TreeNode<SiteMapNode> Tree { get; set; }

        public SelectList ParentSelectList
        {
            get
            { return Tree.ToSelectList(); }
        }

        public MvcHtmlString TreeJson
        {
            get { return new MvcHtmlString( Tree.ToJson() ); }
        }

    }
}