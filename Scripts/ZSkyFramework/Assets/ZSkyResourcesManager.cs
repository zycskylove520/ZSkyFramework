using System;
using System.Collections;
using UnityEngine;
using ZSkyFramework.Core.Singleton;
using ZSkyFramework.Mono;

namespace ZSkyFramework.Assets
{
    /// <summary>
    /// ZSkyResourcesManager用于加载Resources目录下的资源
    /// </summary>
    public class ZSkyResourcesManager : ZSkySingleton<ZSkyResourcesManager>
    {
        /// <summary>
        /// 同步加载Resources目录下的资源
        /// </summary>
        /// <param name="name">Resources目录下的路径+资源名</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        public T LoadResource<T>(string name) where T : UnityEngine.Object
        {
            T res = Resources.Load<T>(name);
            //如果对象是一个GameObject类型的 我把他实例化后再返回出去外部直接使用即可
            if (res is GameObject)
                return UnityEngine.Object.Instantiate(res);
            // 其他类型就直接返回
            return res;
        }
        
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="name">Resources目录下的路径+资源名</param>
        /// <param name="onLoadingFunc">资源异步加载过程中触发的事件</param>
        /// <param name="onLoadEndFunc">资源异步加载结束后触发的事件</param>
        /// <typeparam name="T">资源类型</typeparam>
        public void LoadResourceAsync<T>(string name, Action<ResourceRequest> onLoadingFunc=null, Action<T> onLoadEndFunc=null)
            where T : UnityEngine.Object
        {
            //开启异步加载的协程
            ZSkyMono.Instance.StartCoroutine(LoadResourceCoroutine(name, onLoadingFunc, onLoadEndFunc));
        }

        /// <summary>
        /// 异步加载资源时触发的协程
        /// </summary>
        /// <param name="name">Resources目录下的路径+资源名</param>
        /// <param name="onLoadingFunc">资源异步加载过程中触发的事件</param>
        /// <param name="onLoadEndFunc">资源异步加载结束后触发的事件</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>迭代器</returns>
        private IEnumerator LoadResourceCoroutine<T>(string name, Action<ResourceRequest> onLoadingFunc,
            Action<T> onLoadEndFunc) where T : UnityEngine.Object
        {
            ResourceRequest rq = Resources.LoadAsync<T>(name);
            while (!rq.isDone)
            {
                // 把场景加载过程中的场景信息传递给外部调用
                onLoadingFunc?.Invoke(rq);
                // 这里面去更新进度
                yield return rq.progress;
            }

            // 记载玩资源后把加载的资源通过事件抛出去给外部使用后
            if (rq.asset is GameObject)
                onLoadEndFunc?.Invoke(UnityEngine.Object.Instantiate(rq.asset) as T);
            else
                onLoadEndFunc?.Invoke(rq.asset as T);
        }
    }
}