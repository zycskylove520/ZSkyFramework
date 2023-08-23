namespace ZSkyFramework.Core.Singleton
{
    public class ZSkySingleton<T> where T : new()
    {
        public static T Instance { get; } = new T();
    }
}