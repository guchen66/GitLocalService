using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 终端模拟器配置界面视图模型
    /// <para>允许用户选择Git Bash使用的终端模拟器</para>
    /// </summary>
    public class TerminalViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：终端模拟器选项
        /// </summary>
        private string _terminalOption;

        /// <summary>
        /// 终端模拟器选项
        /// <para>可选值：</para>
        /// <para>- MinTTY：使用MinTTY（Git Bash默认终端）</para>
        /// <para>- WindowsConsole：使用Windows默认控制台</para>
        /// <para>默认值："MinTTY"</para>
        /// </summary>
        public string TerminalOption
        {
            get => _terminalOption;
            set => SetProperty(ref _terminalOption, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public TerminalViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _terminalOption = _config.TerminalOption;
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.TerminalOption = _terminalOption;
        }
    }
}