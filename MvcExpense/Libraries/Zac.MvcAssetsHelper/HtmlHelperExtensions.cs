using Zac.MvcAssetsHelper;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static AssetsHelper Assets( this HtmlHelper htmlHelper )
        {
            return AssetsHelper.GetInstance( htmlHelper );
        }
    }
}