using UnityEngine.UI;
using ZSkyFramework.UI;

namespace ZSkyFramework.Example.UIExample
{
    public class MainPanel : ZSkyBaseUIPanel
    {
        public Button btnRole;

        protected override void Init()
        {
            btnRole.onClick.AddListener(BtnClick);
        }

        protected override void RegisterCommand()
        {
        }

        protected override void UnRegisterCommand()
        {
        }

        private void BtnClick()
        {
            ZSkyUIManager.Instance.ShowPanel<RolePanel>("UI/RolePanel", null);
            ZSkyUIManager.Instance.HidePanel("UI/MainPanel");
        }
    }
}