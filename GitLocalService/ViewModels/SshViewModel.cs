using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// SSH可执行文件选择界面视图模型
    /// <para>允许用户选择Git使用的SSH客户端</para>
    /// </summary>
    public class SshViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：SSH选项
        /// </summary>
        private string _sshOption;

        /// <summary>
        /// SSH选项
        /// <para>可选值：</para>
        /// <para>- BundledOpenSSH：使用Git自带的OpenSSH</para>
        /// <para>- ExternalOpenSSH：使用外部安装的OpenSSH</para>
        /// <para>默认值："BundledOpenSSH"</para>
        /// </summary>
        public string SshOption
        {
            get => _sshOption;
            set => SetProperty(ref _sshOption, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public SshViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _sshOption = _config.SshOption;
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.SshOption = _sshOption;
        }
    }
}