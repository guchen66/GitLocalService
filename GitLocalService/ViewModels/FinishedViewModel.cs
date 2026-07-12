using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 安装完成界面视图模型
    /// <para>允许用户选择安装完成后的操作</para>
    /// </summary>
    public class FinishedViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：是否启动Git Bash
        /// </summary>
        private bool _launchGitBash;

        /// <summary>
        /// 私有字段：是否查看发行说明
        /// </summary>
        private bool _viewReleaseNotes;

        /// <summary>
        /// 是否在安装完成后启动Git Bash
        /// <para>默认值：true</para>
        /// </summary>
        public bool LaunchGitBash
        {
            get => _launchGitBash;
            set => SetProperty(ref _launchGitBash, value);
        }

        /// <summary>
        /// 是否在安装完成后查看发行说明
        /// <para>默认值：true</para>
        /// </summary>
        public bool ViewReleaseNotes
        {
            get => _viewReleaseNotes;
            set => SetProperty(ref _viewReleaseNotes, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public FinishedViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _launchGitBash = _config.LaunchGitBash;
            _viewReleaseNotes = _config.ViewReleaseNotes;
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"Finish"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.LaunchGitBash = _launchGitBash;
            _config.ViewReleaseNotes = _viewReleaseNotes;
        }
    }
}