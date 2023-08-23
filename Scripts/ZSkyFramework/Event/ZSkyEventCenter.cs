using System;
using System.Collections.Generic;
using UnityEngine;
using ZSkyFramework.Core.Singleton;

namespace ZSkyFramework.Event
{
    /// <summary>
    /// 普通事件信息类，不可以传递事件触发者信息
    /// </summary>
    public class SkyEventInfo : IZSkyEventInfo
    {
        public Action Actions;

        public SkyEventInfo(Action action)
        {
            Actions += action;
        }
    }

    /// <summary>
    /// 泛型事件信息类，可以传递事件触发者信息
    /// </summary>
    public class SkyEventInfo<T> : IZSkyEventInfo
    {
        public Action<T> Actions;

        public SkyEventInfo(Action<T> action)
        {
            Actions += action;
        }
    }


    /// <summary>
    /// 事件中心 单例模式对象
    /// </summary>
    public class ZSkyEventCenter : ZSkySingleton<ZSkyEventCenter>
    {
        //key —— 事件的名字（比如：怪物死亡，玩家死亡，通关 等等）
        //value —— 对应的是监听这个事件对应的委托函数们
        private readonly Dictionary<string, IZSkyEventInfo> _eventDic = new Dictionary<string, IZSkyEventInfo>();

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">准备用来处理事件的委托函数</param>
        public void AddEventListener<T>(string name, Action<T> action)
        {
            //有没有对应的事件监听
            //有的情况
            if (_eventDic.TryGetValue(name, out var eventInfo))
            {
                (eventInfo as SkyEventInfo<T>)!.Actions += action;
            }
            //没有的情况
            else
            {
                _eventDic.Add(name, new SkyEventInfo<T>(action));
            }
        }

        /// <summary>
        /// 监听不需要参数传递的事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void AddEventListener(string name, Action action)
        {
            //有没有对应的事件监听
            //有的情况
            if (_eventDic.TryGetValue(name, out var eventInfo))
            {
                (eventInfo as SkyEventInfo)!.Actions += action;
            }
            //没有的情况
            else
            {
                _eventDic.Add(name, new SkyEventInfo(action));
            }
        }

        /// <summary>
        /// 移除对应的事件监听
        /// </summary>
        /// <param name="name">事件的名字</param>
        /// <param name="action">对应之前添加的委托函数</param>
        public void RemoveEventListener<T>(string name, Action<T> action)
        {
            if (_eventDic.TryGetValue(name, out var eventInfo))
                (eventInfo as SkyEventInfo<T>)!.Actions -= action;
        }

        /// <summary>
        /// 移除不需要参数的事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void RemoveEventListener(string name, Action action)
        {
            Debug.Log("yes!remove!");
            if (_eventDic.TryGetValue(name, out var eventInfo))
                (eventInfo as SkyEventInfo)!.Actions -= action;
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="name">哪一个名字的事件触发了</param>
        /// <param name="info">被触发对象携带的参数</param>
        public void EventTrigger<T>(string name, T info)
        {
            //有没有对应的事件监听
            //有的情况
            if (_eventDic.TryGetValue(name, out var eventInfo))
            {
                (eventInfo as SkyEventInfo<T>)!.Actions?.Invoke(info);
                //eventDic[name].Invoke(info);
            }
        }

        /// <summary>
        /// 事件触发（不需要参数的）
        /// </summary>
        /// <param name="name"></param>
        public void EventTrigger(string name)
        {
            //有没有对应的事件监听
            //有的情况
            if (_eventDic.TryGetValue(name, out var eventInfo))
            {
                (eventInfo as SkyEventInfo)!.Actions?.Invoke();
            }
        }

        /// <summary>
        /// 清空事件中心
        /// 主要用在场景切换时
        /// </summary>
        public void Clear()
        {
            _eventDic.Clear();
        }
    }
}