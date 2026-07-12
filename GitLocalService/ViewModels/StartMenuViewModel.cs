using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 开始菜单文件夹界面视图模型
    /// <para>允许用户指定开始菜单中Git快捷方式的文件夹名称</para>
    /// </summary>
    public class StartMenuViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：开始菜单文件夹名称
        /// </summary>
        private string _startMenuFolder;

        /// <summary>
        /// 私有字段：是否创建开始菜单文件夹
        /// </summary>
        private bool _createStartMenuFolder = true;

        /// <summary>
        /// 开始菜单文件夹名称
        /// <para>默认值："Git"</para>
        /// </summary>
        public string StartMenuFolder
        {
            get => _startMenuFolder;
            set => SetProperty(ref _startMenuFolder, value);
        }

        /// <summary>
        /// 是否创建开始菜单文件夹
        /// <para>为true时在开始菜单中创建指定名称的文件夹</para>
        /// <para>默认值：true</para>
        /// </summary>
        public bool CreateStartMenuFolder
        {
            get => _createStartMenuFolder;
            set => SetProperty(ref _createStartMenuFolder, value);
        }

        /// <summary>
        /// 浏览命令（预留）
        /// <para>当前未实现功能</para>
        /// </summary>
        public DelegateCommand BrowseCommand { get; }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public StartMenuViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _startMenuFolder = _config.StartMenuFolder;
            BrowseCommand = new DelegateCommand(Browse);
        }

        /// <summary>
        /// 浏览方法（预留）
        /// <para>当前未实现功能</para>
        /// </summary>
        private void Browse()
        {
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.StartMenuFolder = _startMenuFolder;
        }
    }
}