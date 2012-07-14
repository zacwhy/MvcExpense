
namespace System.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Title( this HtmlHelper htmlHelper, string viewBagTitle )
        {
            return Title( htmlHelper, SiteMap.CurrentNode, viewBagTitle );
        }

        public static MvcHtmlString Title( this HtmlHelper htmlHelper, SiteMapNode currentNode, string viewBagTitle )
        {
            string title = currentNode != null ? currentNode.Title : viewBagTitle;
            return MvcHtmlString.Create( title );
        }
    }
}