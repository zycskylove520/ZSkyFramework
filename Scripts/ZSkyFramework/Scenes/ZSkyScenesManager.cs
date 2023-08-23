using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZSkyFramework.Core.Singleton;
using ZSkyFramework.Mono;

namespace ZSkyFramework.Scenes
{
    /// <summary>
    /// ZSkyScenesManager用于加载场景，支持同步和异步加载
    /// </summary>
    public class ZSkyScenesManager : ZSkySingleton<ZSkyScenesManager>
    {
        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="sceneName">要加载的场景的名字</param>
        /// <param name="onLoadEndFunc">场景加载结束后要触发的事件</param>
        public void LoadScene(string sceneName, Action onLoadEndFunc=null)
        {
            // 场景同步加载
            SceneManager.LoadScene(sceneName);
            // 场景加载完成过后触发的事件
            onLoadEndFunc?.Invoke();
        }

        /// <summary>
        /// 场景异步加载
        /// </summary>
        /// <param name="sceneName">要加载的场景的名字</param>
        /// <param name="onLoadingFunc">场景异步加载过程中要触发的事件</param>
        /// <param name="onLoadEndFunc">场景异步加载结束后要触发的事件</param>
        public void LoadSceneAsync(string sceneName, Action<AsyncOperation> onLoadingFunc=null, Action onLoadEndFunc=null)
        {
            ZSkyMono.Instance.StartCoroutine(LoadSceneAsyncCoroutine(sceneName, onLoadingFunc, onLoadEndFunc));
        }

        /// <summary>
        /// 异步加载场景时触发的协程
        /// </summary>
        /// <param name="sceneName">要加载的场景的名字</param>
        /// <param name="onLoadingFunc">场景异步加载过程中要触发的事件</param>
        /// <param name="onLoadEndFunc">场景异步加载结束后要触发的事件</param>
        /// <returns>迭代器</returns>
        private IEnumerator LoadSceneAsyncCoroutine(string sceneName, Action<AsyncOperation> onLoadingFunc, Action onLoadEndFunc)
        {
            // 场景异步加载
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
            // 可以得到场景加载的一个进度
            while (!ao.isDone)
            {
                // 把场景加载过程中的场景信息传递给外部调用
                onLoadingFunc?.Invoke(ao);
                // 这里面去更新进度
                yield return ao.progress;
            }

            // 场景加载完成过后触发的事件
            onLoadEndFunc?.Invoke();
        }
    }
}