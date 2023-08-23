using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZSkyFramework.Assets;
using ZSkyFramework.Core.Singleton;
using ZSkyFramework.Mono;
using Object = UnityEngine.Object;

namespace ZSkyFramework.Pool
{
    /// <summary>
    /// GameObject对象缓存池
    /// </summary>
    public class ZSkyGameObjectPool : ZSkySingleton<ZSkyGameObjectPool>, IZSkyPool<GameObject>
    {
        /// <summary>
        /// 缓存池容器,保存了各种不同的GameObject的缓存池
        /// </summary>
        private readonly Dictionary<string, List<GameObject>> _gameObjectContainer =
            new Dictionary<string, List<GameObject>>();

        /// <summary>
        /// 缓存池根对象，当对象回收时，保存在该根对象下方
        /// </summary>
        private GameObject _rootObject;

        /// <summary>
        /// 从缓存池获取一个GameObject，注意获取到GameObject对象后要进行初始化！
        /// </summary>
        /// <param name="poolName">游戏对象缓存池的名字，命名规则为存储在Resources目录下的路径+prefab名字</param>
        /// <returns>从缓存池获得的GameObject</returns>
        public GameObject GetObjectFromPool(string poolName)
        {
            GameObject obj;
            // 缓存池中有该对象
            if (_gameObjectContainer.ContainsKey(poolName) && _gameObjectContainer[poolName].Count > 0)
            {
                obj = _gameObjectContainer[poolName][0];
                _gameObjectContainer[poolName].RemoveAt(0);
            }
            else // 缓存池中没有该对象
            {
                obj = ZSkyResourcesManager.Instance.LoadResource<GameObject>(poolName);
                obj.name = poolName;
            }

            obj.transform.parent = null; // 把对象移到根对象外面
            obj.SetActive(true); // 激活对象在场景中
            return obj;
        }

        /// <summary>
        /// 从缓存池异步获取一个GameObject，注意获取到GameObject对象后要进行初始化！
        /// </summary>
        /// <param name="poolName">游戏对象缓存池的名字，命名规则为存储在Resources目录下的路径+prefab名字</param>
        /// <param name="onGetEndFunc">GameObject异步加载结束后可通过委托获取加载的游戏对象</param>
        /// <returns></returns>
        public void GetObjectFromPoolAsync(string poolName, Action<GameObject> onGetEndFunc)
        {
            // 缓存池中有该对象
            if (_gameObjectContainer.ContainsKey(poolName) && _gameObjectContainer[poolName].Count > 0)
            {
                GameObject obj = _gameObjectContainer[poolName][0];
                _gameObjectContainer[poolName].RemoveAt(0);
                obj.transform.parent = null; // 把对象移到根对象外面
                obj.SetActive(true); // 激活对象在场景中

                onGetEndFunc(obj); // 通过委托返回给外部使用
            }
            else // 缓存池中没有该对象
            {
                ZSkyResourcesManager.Instance.LoadResourceAsync<GameObject>(poolName, null, (obj) =>
                {
                    obj.name = poolName; // 修改GameObject的名字
                    obj.transform.parent = null; // 把对象移到根对象外面
                    obj.SetActive(true); // 激活对象在场景中
                    onGetEndFunc(obj);
                });
            }
        }

        /// <summary>
        /// 将一个GameObject推入缓存池
        /// </summary>
        /// <param name="poolName">游戏对象在缓存池的存储的名字</param>
        /// <param name="obj">要存入缓存池的GameObject</param>
        public void PushObjectToPool(string poolName, GameObject obj)
        {
            // 如果不存在根对象，那就创建，并把所有失活对象储存在该根对象下面
            _rootObject ??= new GameObject("Pool");
            _rootObject.transform.position = Vector3.zero;

            obj.transform.parent = _rootObject.transform;

            // 失活隐藏对象在场景中
            obj.SetActive(false);
            // 如果缓存池中有这个对象
            if (_gameObjectContainer.TryGetValue(poolName, out var objectList))
            {
                objectList.Add(obj);
            }
            else
            {
                // 如果缓存池中没有这个对象，那就创建这个对象的池子
                _gameObjectContainer.Add(poolName, new List<GameObject>() { obj });
            }
        }

        /// <summary>
        /// 获取容器里的缓存池的个数
        /// </summary>
        /// <returns>容器里现有的缓存池的个数</returns>
        public int GetPoolCount()
        {
            return _gameObjectContainer.Count;
        }

        /// <summary>
        /// 获取缓存池里的GameObject的个数
        /// </summary>
        /// <param name="poolName">GameObject池的名字</param>
        /// <returns>缓存池里GameObject的个数</returns>
        public int GetGameObjectCountFromPool(string poolName)
        {
            if (_gameObjectContainer.TryGetValue(poolName, out var value))
            {
                return value.Count;
            }

            return 0;
        }

        /// <summary>
        /// 返回所有的缓存池的名字
        /// </summary>
        /// <returns>所有的缓存池的名字</returns>
        public IEnumerable<string> GetPoolName()
        {
            return _gameObjectContainer.Keys;
        }

        /// <summary>
        /// 清空一个GameObject池
        /// 注意：这是个开销很大的方法，可能会造成卡顿，请考虑使用异步的ClearPoolAsync方法
        /// </summary>
        /// <param name="poolName">GameObject池的名字</param>
        public void ClearPool(string poolName)
        {
            if (_gameObjectContainer.TryGetValue(poolName, out var pool))
            {
                // 要先摧毁缓存池里所有的GameObject对象，再清空池子
                foreach (GameObject gameObject in _gameObjectContainer[poolName])
                {
                    Object.Destroy(gameObject);
                }

                pool.Clear();
            }
        }

        /// <summary>
        /// 异步清空一个GameObject池
        /// 注意：该方法并不安全，保证在调用时没有别的对象访问该GameObject池
        /// </summary>
        /// <param name="poolName">GameObject池的名字</param>
        public void ClearPoolAsync(string poolName)
        {
            ZSkyMono.Instance.StartCoroutine(ClearPoolCoroutine(poolName));
        }

        /// <summary>
        /// 清空整个GameObject容器
        /// 注意：这是个开销很大的方法，可能会造成卡顿，请考虑使用异步的ClearContainerAsync方法
        /// </summary>
        public void ClearContainer()
        {
            foreach (List<GameObject> pool in _gameObjectContainer.Values)
            {
                foreach (GameObject gameObject in pool)
                {
                    Object.Destroy(gameObject); // 摧毁每一个GameObject对象
                }

                pool.Clear();
            }

            _gameObjectContainer.Clear();
        }

        /// <summary>
        /// 异步清空整个GameObject容器
        /// 注意：该方法并不安全，保证在调用时没有别的对象访问GameObject容器
        /// </summary>
        public void ClearContainerAsync()
        {
            ZSkyMono.Instance.StartCoroutine(ClearContainerCoroutine());
        }

        /// <summary>
        /// 从容器中移除某个GameObject池
        /// </summary>
        /// <param name="poolName">GameObject池的名字</param>
        public void DeletePool(string poolName)
        {
            if (_gameObjectContainer.ContainsKey(poolName))
            {
                // 要先摧毁缓存池里所有的GameObject对象，再删除池子
                foreach (GameObject gameObject in _gameObjectContainer[poolName])
                {
                    Object.Destroy(gameObject);
                }

                _gameObjectContainer[poolName].Clear(); // 先清空
                _gameObjectContainer.Remove(poolName); // 再删除
            }
        }

        /// <summary>
        /// 清空池子的协程
        /// </summary>
        /// <param name="poolName">GameObject池的名字</param>
        /// <returns></returns>
        private IEnumerator ClearPoolCoroutine(string poolName)
        {
            if (_gameObjectContainer.TryGetValue(poolName, out var pool))
            {
                // 要先摧毁缓存池里所有的GameObject对象，再清空池子
                foreach (GameObject gameObject in _gameObjectContainer[poolName])
                {
                    Object.Destroy(gameObject);
                    yield return null; // 每帧销毁一个gameObject
                }

                pool.Clear();
            }
        }

        /// <summary>
        /// 清空容器的协程
        /// </summary>
        /// <returns></returns>
        private IEnumerator ClearContainerCoroutine()
        {
            foreach (KeyValuePair<string, List<GameObject>> pool in _gameObjectContainer)
            {
                ClearPoolAsync(pool.Key);
                yield return null;
            }

            _gameObjectContainer.Clear();
        }
    }
}