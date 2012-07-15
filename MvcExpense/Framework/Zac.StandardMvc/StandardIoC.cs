using StructureMap;

namespace Zac.StandardMvc
{
    public static class StandardIoC
    {
        public static IContainer Container { get; set; }

        internal static T GetInstance<T>()
        {
            return Container.GetInstance<T>();
        }

    }
}
