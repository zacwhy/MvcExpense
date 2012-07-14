using System.Web.Mvc;

namespace Zac.MvcAssetsHelper
{
    public class AssetsHelper
    {
        public static AssetsHelper GetInstance(HtmlHelper htmlHelper)
        {
            var instanceKey = "AssetsHelperInstance";

            var context = htmlHelper.ViewContext.HttpContext;
            if (context == null) return null;

            var assetsHelper = (AssetsHelper)context.Items[instanceKey];

            if (assetsHelper == null)
                context.Items.Add(instanceKey, assetsHelper = new AssetsHelper());

            return assetsHelper;
        }

        public ItemRegistrar Styles { get; private set; }
        public ItemRegistrar Scripts { get; private set; }

        public AssetsHelper()
        {
            Styles = new ItemRegistrar(ItemRegistrarFormatters.StyleFormat);
            Scripts = new ItemRegistrar(ItemRegistrarFormatters.ScriptFormat);
        }
    }

}