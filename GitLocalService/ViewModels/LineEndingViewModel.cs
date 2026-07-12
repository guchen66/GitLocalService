using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 换行符转换配置界面视图模型
    /// <para>允许用户配置Git处理文本文件换行符的方式</para>
    /// </summary>
    public class LineEndingViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：换行符转换选项
        /// </summary>
        private string _lineEndingOption;

        /// <summary>
        /// 换行符转换选项
        /// <para>可选值：</para>
        /// <para>- WindowsCheckoutUnixCommit：检出时转换为Windows格式，提交时转换为Unix格式（推荐）</para>
        /// <para>- AsIs：保持原样，检出和提交都不转换</para>
        /// <para>- Unix：检出和提交都使用Unix格式</para>
        /// <para>默认值："WindowsCheckoutUnixCommit"</para>
        /// </summary>
        public string LineEndingOption
        {
            get => _lineEndingOption;
            set => SetProperty(ref _lineEndingOption, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public LineEndingViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _lineEndingOption = _config.LineEndingOption;
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.LineEndingOption = _lineEndingOption;
        }
    }
}