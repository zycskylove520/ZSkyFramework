using System;
using UnityEngine;
using ZSkyFramework.Example.UIExample;
using ZSkyFramework.UI;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        ZSkyUIManager.Instance.ShowPanel<MainPanel>("UI/MainPanel", null);
    }
}