using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 额外选项配置界面视图模型
    /// <para>允许用户配置Git的高级选项</para>
    /// </summary>
    public class ExtraOptionsViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：是否启用文件系统缓存
        /// </summary>
        private bool _enableFileSystemCaching;

        /// <summary>
        /// 私有字段：是否启用符号链接
        /// </summary>
        private bool _enableSymbolicLinks;

        /// <summary>
        /// 私有字段：是否启用伪控制台支持
        /// </summary>
        private bool _enablePseudoConsole;

        /// <summary>
        /// 是否启用文件系统缓存
        /// <para>启用后可提高Git操作性能</para>
        /// <para>默认值：true</para>
        /// </summary>
        public bool EnableFileSystemCaching
        {
            get => _enableFileSystemCaching;
            set => SetProperty(ref _enableFileSystemCaching, value);
        }

        /// <summary>
        /// 是否启用符号链接支持
        /// <para>启用后Git可以创建和处理符号链接</para>
        /// <para>需要管理员权限</para>
        /// <para>默认值：true</para>
        /// </summary>
        public bool EnableSymbolicLinks
        {
            get => _enableSymbolicLinks;
            set => SetProperty(ref _enableSymbolicLinks, value);
        }

        /// <summary>
        /// 是否启用伪控制台支持
        /// <para>在Windows 10 1809+上提供更好的终端体验</para>
        /// <para>默认值：true</para>
        /// </summary>
        public bool EnablePseudoConsole
        {
            get => _enablePseudoConsole;
            set => SetProperty(ref _enablePseudoConsole, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public ExtraOptionsViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _enableFileSystemCaching = _config.EnableFileSystemCaching;
            _enableSymbolicLinks = _config.EnableSymbolicLinks;
            _enablePseudoConsole = _config.EnablePseudoConsole;
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.EnableFileSystemCaching = _enableFileSystemCaching;
            _config.EnableSymbolicLinks = _enableSymbolicLinks;
            _config.EnablePseudoConsole = _enablePseudoConsole;
        }
    }
}