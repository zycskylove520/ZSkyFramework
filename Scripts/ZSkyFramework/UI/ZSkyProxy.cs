namespace ZSkyFramework.UI
{
    public abstract class ZSkyProxy<T> where T : class, new()
    {
        protected readonly T Model;

        protected ZSkyProxy()
        {
            Model = new T();
            InitModelData();
        }

        protected abstract void InitModelData();

        public abstract void SaveModelData();
    }
}