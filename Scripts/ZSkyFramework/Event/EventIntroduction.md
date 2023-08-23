# ZSky事件模块使用指南

## 事件触发者
***
对于触发者而言，在使用事件时，可以在任何阶段触发事件。具体根据游戏对象的触发阶段来自行判断。

## 事件监听者
***
### 缓存池对象
缓存池中的对象默认情况下不会主动销毁，而是会被失活在游戏中，因此在添加事件监听时，推荐在<b>OnEnable</b>函数中添加事件监听， 在<b>OnDisable</b>函数中取消事件监听。
```C#
private void OnEnable()
{
    ZSkyEventCenter.Instance.AddEventListener("Dead", DeadEvent);
}

private void OnDisable()
{  
    ZSkyEventCenter.Instance.RemoveEventListener("Dead", DeadEvent);
}

public void DeadEvent()
{
}
```

### 普通MonoBehavior脚本对象
继承自MonoBehavior的脚本对象分情况来进行监听。、

#### 游戏中频繁隐藏和显示的对象 
如果一个游戏对象不希望它在被失活时依旧能监听到事件，那么推荐在<b>OnEnable</b>函数中添加事件监听，在<b>OnDisable</b>函数中取消事件监听。
```C#
private void OnEnable()
{
    ZSkyEventCenter.Instance.AddEventListener("Dead", DeadEvent);
}

private void OnDisable()
{  
    ZSkyEventCenter.Instance.RemoveEventListener("Dead", DeadEvent);
}

public void DeadEvent()
{
}
```

#### 游戏中会被销毁的对象
该类对象在添加事件监听时，推荐在<b>OnAwake</b>函数中添加事件监听，在<b>OnDestroy</b>函数中取消事件监听。
```C#
private void OnAwake()
{
    ZSkyEventCenter.Instance.AddEventListener("Dead", DeadEvent);
}

private void OnDestroy()
{  
    ZSkyEventCenter.Instance.RemoveEventListener("Dead", DeadEvent);
}

public void DeadEvent()
{
}
```

### 普通C#脚本对象
自己决定在何时添加事件监听与销毁。