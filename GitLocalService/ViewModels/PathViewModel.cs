using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// PATH环境变量配置界面视图模型
    /// <para>允许用户配置Git在系统PATH中的可见性</para>
    /// </summary>
    public class PathViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：PATH环境变量配置选项
        /// </summary>
        private string _pathOption;

        /// <summary>
        /// PATH环境变量配置选项
        /// <para>可选值：</para>
        /// <para>- GitBashOnly：仅在Git Bash中使用Git</para>
        /// <para>- CmdAndThirdParty：在命令行和第三方软件中使用Git（推荐）</para>
        /// <para>- CmdAndUnixTools：在命令行中使用Git和Unix工具</para>
        /// <para>默认值："CmdAndThirdParty"</para>
        /// </summary>
        public string PathOption
        {
            get => _pathOption;
            set => SetProperty(ref _pathOption, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public PathViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _pathOption = _config.PathOption;
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.PathOption = _pathOption;
        }
    }
}