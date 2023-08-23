using UnityEngine;

namespace ZSkyFramework.Core.Singleton
{
    public class ZSkyMonoSingleton<T> : MonoBehaviour where T: ZSkyMonoSingleton<T>
    {
        private static T _instance;
        public static T Instance => _instance;

        private void Awake()
        {
            // 单例模式只允许场景中存在一个该脚本
            if (_instance is null)
            {
                _instance = (T)this;  // 没有则创建
            }
            else
            {
                Destroy(this);  // 有则摧毁该多余的脚本
            }
        }
        
        /// <summary>
        /// 判断单例是否已经初始化了
        /// </summary>
        public static bool IsInitialized => _instance != null;

        private void OnDestroy()
        {
            if(_instance == this)
            {
                _instance = null;
            }
        }
    }
}