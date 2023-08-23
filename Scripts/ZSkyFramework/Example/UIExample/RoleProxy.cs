using ZSkyFramework.Event;
using ZSkyFramework.UI;

namespace ZSkyFramework.Example.UIExample
{
    public class RoleProxy:ZSkyProxy<RoleModel>
    {
        /// <summary>
        /// 公开的HP属性
        /// 1.获取模型的hp值
        /// 2.当hp值改变时，更新私有的hp的同时，触发命令，通知面板进行更新或通知别的什么
        /// </summary>
        public int Hp
        {
            get => Model.Hp;
            set
            {
                Model.Hp = value;
                ZSkyEventCenter.Instance.EventTrigger("HpChangedCommand", value);
            }
        }
        
        /// <summary>
        /// 必须存在该参数初始化模型数据
        /// </summary>
        protected override void InitModelData()
        {
            Hp = 1;
        }

        /// <summary>
        /// 保存模型数据方法
        /// </summary>
        public override void SaveModelData()
        {
            
        }
    }
}