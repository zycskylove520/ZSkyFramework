namespace ZSkyFramework.Pool
{
    public interface IZSkyPool<T>
    {
        /// <summary>
        /// 从缓存池获取一个object
        /// </summary>
        /// <param name="poolName">object名字</param>
        /// <returns></returns>
        T GetObjectFromPool(string poolName);

        /// <summary>
        /// 把一个object推入缓存池
        /// </summary>
        /// <param name="poolName">object名字</param>
        /// <param name="obj">要推入的object</param>
        void PushObjectToPool(string poolName, T obj);

        /// <summary>
        /// 清空某个object缓存池
        /// </summary>
        /// <param name="poolName"></param>
        void ClearPool(string poolName);

        /// <summary>
        /// 清空整个容器
        /// </summary>
        void ClearContainer();
    }
}