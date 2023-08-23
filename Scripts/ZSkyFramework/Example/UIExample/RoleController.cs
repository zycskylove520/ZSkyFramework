namespace ZSkyFramework.Example.UIExample
{
    /// <summary>
    /// 控制器用于和面板进行交互
    /// </summary>
    public class RoleController
    {
        /// <summary>
        /// 注入Model数据
        /// </summary>
        private readonly RoleProxy _roleProxy;

        public RoleController()
        {
            // 构造时进行初始化，注意RoleModel初始化时会同时进行内部数据初始化
            _roleProxy = new RoleProxy();
        }

        /// <summary>
        /// UI面板上的控件触发的回调方法
        /// </summary>
        public void UpdatePanelData()
        {
            // Hp增加会触发HpChangedCommand命令
            _roleProxy.Hp ++;
        }
    }
}