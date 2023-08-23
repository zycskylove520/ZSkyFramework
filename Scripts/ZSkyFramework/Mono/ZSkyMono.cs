using System;
using System.Collections;
using UnityEngine;
using ZSkyFramework.Core.Singleton;

namespace ZSkyFramework.Mono
{
    /// <summary>
    /// ZSkyMono类用于更新非继承MonoBehavior的类
    /// 可以提供给外部添加帧更新事件的方法
    /// 可以提供给外部添加协程的方法
    /// </summary>
    public class ZSkyMono : ZSkySingleton<ZSkyMono>
    {
        private readonly ZSkyMonoCenter _center;

        /// <summary>
        /// 在场景中创建ZSkyMonoCenter对象，用于持续更新事件
        /// </summary>
        public ZSkyMono()
        {
            //保证了MonoController对象的唯一性
            GameObject obj = new GameObject("ZSkyMonoCenter");
            _center = obj.AddComponent<ZSkyMonoCenter>();
        }

        /// <summary>
        /// 添加帧更新事件的函数
        /// </summary>
        /// <param name="fun">监听的事件</param>
        public void AddUpdateListener(Action fun)
        {
            _center.AddUpdateListener(fun);
        }

        /// <summary>
        /// 帧更新事件函数
        /// </summary>
        /// <param name="fun">监听的事件</param>
        public void RemoveUpdateListener(Action fun)
        {
            _center.RemoveUpdateListener(fun);
        }

        /// <summary>
        /// 清空所有事件
        /// </summary>
        public void ClearEvent()
        {
            _center.ClearEvent();
        }

        /// <summary>
        /// 启动协程
        /// </summary>
        /// <param name="routine">协程</param>
        /// <returns></returns>
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return _center.StartCoroutine(routine);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="routine">协程</param>
        public void StopCoroutine(IEnumerator routine)
        {
            _center.StopCoroutine(routine);
        }

        /// <summary>
        /// 停止所有的协程
        /// </summary>
        public void StopAllCoroutines()
        {
            _center.StopAllCoroutines();
        }
        
        
    }
}