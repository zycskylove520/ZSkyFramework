using UnityEngine.Serialization;
using UnityEngine.UI;
using ZSkyFramework.Event;
using ZSkyFramework.UI;

namespace ZSkyFramework.Example.UIExample
{
    /// <summary>
    /// 这是一个角色面板，包含一个升级按钮，按下升级按钮会使角色面板上的hp增加1
    /// </summary>
    public class RolePanel : ZSkyBaseUIPanel
    {
        // 注入该面板对应的控制器
        private RoleController _roleController;

        // 面板上的升级按钮控件
        public Button levelUp;

        // 面板上的HP文本控件
        public Text hp;

        /// <summary>
        /// 所有UI控件添加事件监听和controller创建都必须在此处进行
        /// </summary>
        protected override void Init()
        {
            // 在Init的时候创建控制器
            // 注意：不可以在Awake时创建控制器
            _roleController = new RoleController();

            // 为升级按钮添加点击事件
            levelUp.onClick.AddListener(LevelUpButtonClick);
        }

        /// <summary>
        /// 注册Model数据改变时触发的命令
        /// </summary>
        protected override void RegisterCommand()
        {
            ZSkyEventCenter.Instance.AddEventListener<int>("HpChangedCommand", HpChangedCommand);
        }

        /// <summary>
        /// 取消注册Model数据改变时触发的命令
        /// </summary>
        protected override void UnRegisterCommand()
        {
            ZSkyEventCenter.Instance.RemoveEventListener<int>("HpChangedCommand", HpChangedCommand);
        }


        /// <summary>
        /// Model数据改变时触发的命令
        /// 更新面板上的hp的文本内容
        /// </summary>
        /// <param name="newHp">改变的新hp值</param>
        private void HpChangedCommand(int newHp)
        {
            hp.text = newHp.ToString();
        }

        // 升级按钮的点击事件
        private void LevelUpButtonClick()
        {
            // 触发控制器里的更新面板数据的方法
            _roleController.UpdatePanelData();
        }
    }
}