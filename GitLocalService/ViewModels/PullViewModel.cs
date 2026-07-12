using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 默认拉取行为配置界面视图模型
    /// <para>允许用户配置git pull命令的默认行为</para>
    /// </summary>
    public class PullViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：默认拉取行为
        /// </summary>
        private string _pullBehavior;

        /// <summary>
        /// 默认拉取行为
        /// <para>可选值：</para>
        /// <para>- Merge：合并（如果可能则快进）</para>
        /// <para>- Rebase：变基</para>
        /// <para>- FastForwardOnly：仅快进</para>
        /// <para>默认值："Merge"</para>
        /// </summary>
        public string PullBehavior
        {
            get => _pullBehavior;
            set => SetProperty(ref _pullBehavior, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public PullViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _pullBehavior = _config.PullBehavior;
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.PullBehavior = _pullBehavior;
        }
    }
}