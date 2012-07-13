using Zac.Mvc.Controllers;

namespace Zac.Mvc
{
    public static class BootStrapper
    {
        public static void CreateAutoMapperMaps()
        {
            SiteMapNodeController.CreateMaps();
        }
    }

}
