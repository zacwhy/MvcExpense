using System.Web.Mvc;
using Zac.StandardMvc.Extensions;
using Zac.StandardMvc.Models.Input;
using Zac.StandardCore.Models;
using Zac.Tree;

namespace Zac.StandardMvc.Models.Display
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