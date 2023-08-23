using System;
using UnityEngine;

namespace ZSkyFramework.Mono
{
    /// <summary>
    /// 管理所有未继承MonoBehavior的类的事件更新
    /// </summary>
    internal class ZSkyMonoCenter : MonoBehaviour
    {
        /// <summary>
        /// 所有未继承MonoBehavior的类都可以在该处添加事件更新
        /// </summary>
        private event Action UpdateEvent;

        /// <summary>
        /// 该模块过场景时防止删除
        /// </summary>
        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// 更新所有的事件
        /// </summary>
        private void Update()
        {
            UpdateEvent?.Invoke();
        }

        /// <summary>
        /// 当该对象被销毁时，释放内置所有挂载事件，防止内存泄露
        /// </summary>
        private void OnDestroy()
        {
            UpdateEvent = delegate { };
        }

        /// <summary>
        /// 添加监听事件
        /// </summary>
        /// <param name="fun">监听的事件</param>
        public void AddUpdateListener(Action fun)
        {
            UpdateEvent += fun;
        }

        /// <summary>
        /// 移除监听事件
        /// </summary>
        /// <param name="fun">监听的事件</param>
        public void RemoveUpdateListener(Action fun)
        {
            UpdateEvent -= fun;
        }
        
        /// <summary>
        /// 手动清空所有事件
        /// </summary>
        public void ClearEvent()
        {
            UpdateEvent = delegate { };
        }
    }
}