using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using ZSkyFramework.Assets;
using ZSkyFramework.Core.Singleton;
using Object = UnityEngine.Object;

namespace ZSkyFramework.UI
{
    public class ZSkyUIManager : ZSkySingleton<ZSkyUIManager>
    {
        /// <summary>
        /// 面板字典，保存所有的面板
        /// </summary>
        private readonly Dictionary<string, ZSkyBaseUIPanel> _panelDict = new Dictionary<string, ZSkyBaseUIPanel>();

        /// <summary>
        /// UI相机组件
        /// </summary>
        private readonly GameObject _uiCamera;

        /// <summary>
        /// UI画布组件
        /// </summary>
        private readonly GameObject _canvas;

        /// <summary>
        /// 事件系统组件
        /// </summary>
        private readonly GameObject _eventSystem;

        public ZSkyUIManager()
        {
            // 1.创建UICanvas，设置过场景不移除UICanvas
            GameObject uiCanvas = ZSkyResourcesManager.Instance.LoadResource<GameObject>("UI/UICanvas");
            Object.DontDestroyOnLoad(uiCanvas);
            // 获取三大子对象
            _canvas = uiCanvas.transform.Find("Canvas").gameObject;
            _uiCamera = uiCanvas.transform.Find("UICamera").gameObject;
            _eventSystem = uiCanvas.transform.Find("EventSystem").gameObject;

            // 2.寻找Main Camera，把UICamera加入到MainCamera的相机堆栈中
            GameObject mainCamera = GameObject.Find("Main Camera");
            if (mainCamera != null)
            {
                mainCamera.GetComponent<Camera>().GetUniversalAdditionalCameraData().cameraStack
                    .Add(_uiCamera.GetComponent<Camera>());
            }
            else
            {
                throw new Exception("Main Camera cannot find!");
            }
        }

        /// <summary>
        /// 显示面板
        /// 注意：该方法为异步的！
        /// </summary>
        /// <typeparam name="T">面板脚本类型</typeparam>
        /// <param name="panelName">面板名称</param>
        /// <param name="onShowEndFunc">当面板预设体创建成功后触发的回调</param>
        public void ShowPanel<T>(string panelName, Action<T> onShowEndFunc = null)
            where T : ZSkyBaseUIPanel
        {
            if (_panelDict.ContainsKey(panelName))
            {
                _panelDict[panelName].Show();

                // 处理面板创建完成后的逻辑
                onShowEndFunc?.Invoke(_panelDict[panelName] as T);
                //避免面板重复加载 如果存在该面板 即直接显示 调用回调函数后  直接return 不再处理后面的异步加载逻辑
                return;
            }

            ZSkyResourcesManager.Instance.LoadResourceAsync<GameObject>(panelName, null, (obj) =>
            {
                //设置父对象  设置相对位置和大小
                obj.transform.SetParent(_canvas.transform, false);

                //得到预设体身上的面板脚本
                T panel = obj.GetComponent<T>();

                // 处理面板创建完成后的逻辑
                onShowEndFunc?.Invoke(panel);

                panel.Show();

                //把面板存起来
                _panelDict.Add(panelName, panel);
            });
        }

        /// <summary>
        /// 隐藏面板
        /// </summary>
        /// <param name="panelName">面板名称</param>
        public void HidePanel(string panelName)
        {
            if (_panelDict.ContainsKey(panelName))
            {
                _panelDict[panelName].Hide();
                Object.Destroy(_panelDict[panelName].gameObject);
                _panelDict.Remove(panelName);
            }
        }

        /// <summary>
        /// 得到某一个已经显示的面板 方便外部使用
        /// </summary>
        /// <typeparam name="T">面板脚本类型</typeparam>
        /// <param name="panelName">面板名称</param>
        public T GetPanel<T>(string panelName) where T : ZSkyBaseUIPanel
        {
            if (_panelDict.TryGetValue(panelName, out var panel))
                return panel as T;
            return null;
        }

        /// <summary>
        /// 允许外界设置UICamera的属性
        /// </summary>
        /// <param name="callBack"></param>
        public void SetUICamera(Action<GameObject> callBack)
        {
            callBack?.Invoke(_uiCamera);
        }

        /// <summary>
        /// 允许外界设置Canvas的属性
        /// </summary>
        /// <param name="callBack"></param>
        public void SetCanvas(Action<GameObject> callBack)
        {
            callBack?.Invoke(_canvas);
        }

        /// <summary>
        /// 允许外界设置EventSystem的属性
        /// </summary>
        /// <param name="callBack"></param>
        public void SetEventSystem(Action<GameObject> callBack)
        {
            callBack?.Invoke(_eventSystem);
        }
    }
}