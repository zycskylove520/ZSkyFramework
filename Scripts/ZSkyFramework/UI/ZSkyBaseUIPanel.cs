using System;
using UnityEngine;

namespace ZSkyFramework.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ZSkyBaseUIPanel : MonoBehaviour
    {
        /// <summary>
        /// 面板自带一个淡入淡出组件
        /// </summary>
        private CanvasGroup _canvasGroup;

        /// <summary>
        /// 每个面板自带一个层级参数，请不要直接设置它的值！
        /// </summary>
        public int LayerIndex => transform.GetSiblingIndex();

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            RegisterCommand();
        }

        protected virtual void Start()
        {
            // 这里可以写基类里一些需要在start时才能做的事情

            // 里面调Init方法，让子类在这个方法里进行面板的初始化
            Init();
        }

        protected virtual void OnDestroy()
        {
            UnRegisterCommand();
        }

        /// <summary>
        /// 因为基类面板不能直接用，所有这里为抽象方法，子类去重写
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 在事件中心注册自定义命令
        /// </summary>
        protected abstract void RegisterCommand();

        /// <summary>
        /// 在事件中心取消命令的注册
        /// </summary>
        protected abstract void UnRegisterCommand();


        /// <summary>
        ///  显示自己的功能，子类可以继承该方法
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏自己的功能，子类可以继承该方法
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}