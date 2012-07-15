using Zac.StandardMvc.Controllers;

namespace Zac.StandardMvc
{
    public static class StandardBootStrapper
    {
        public static void CreateAutoMapperMaps()
        {
            SiteMapNodeController.CreateMaps();
        }
    }

}
