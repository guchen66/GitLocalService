using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 初始分支名称界面视图模型
    /// <para>允许用户配置Git仓库的初始分支名称</para>
    /// </summary>
    public class BranchViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：是否使用自定义分支名称
        /// </summary>
        private bool _useCustomBranchName;

        /// <summary>
        /// 私有字段：初始分支名称
        /// </summary>
        private string _initialBranchName;

        /// <summary>
        /// 是否使用自定义分支名称
        /// <para>为true时使用InitialBranchName指定的名称</para>
        /// <para>为false时使用Git默认行为（通常为master）</para>
        /// <para>默认值：false</para>
        /// </summary>
        public bool UseCustomBranchName
        {
            get => _useCustomBranchName;
            set => SetProperty(ref _useCustomBranchName, value);
        }

        /// <summary>
        /// 初始分支名称
        /// <para>当UseCustomBranchName为true时生效</para>
        /// <para>常见值：master, main, trunk等</para>
        /// <para>默认值："master"</para>
        /// </summary>
        public string InitialBranchName
        {
            get => _initialBranchName;
            set => SetProperty(ref _initialBranchName, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public BranchViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _useCustomBranchName = _config.UseCustomBranchName;
            _initialBranchName = _config.InitialBranchName;
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.UseCustomBranchName = _useCustomBranchName;
            _config.InitialBranchName = _initialBranchName;
        }
    }
}