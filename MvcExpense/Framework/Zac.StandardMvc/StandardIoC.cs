using StructureMap;

namespace Zac.StandardMvc
{
    public static class StandardIoC
    {
        private static IContainer _container;

        public static void SetContainer( IContainer container )
        {
            _container = container;
        }

        internal static T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }

    }
}
