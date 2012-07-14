using Zac.StandardMvc.Controllers;

namespace Zac.StandardMvc
{
    public static class BootStrapper
    {
        public static void CreateAutoMapperMaps()
        {
            SiteMapNodeController.CreateMaps();
        }
    }

}
