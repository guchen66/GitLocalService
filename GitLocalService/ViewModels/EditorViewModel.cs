using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 默认编辑器界面视图模型
    /// <para>允许用户选择Git使用的默认文本编辑器</para>
    /// </summary>
    public class EditorViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：默认编辑器名称
        /// </summary>
        private string _defaultEditor;

        /// <summary>
        /// 默认编辑器名称
        /// <para>可选值：Vim, Nano, Notepad, Notepad++, VS Code, Visual Studio等</para>
        /// <para>默认值："Vim"</para>
        /// </summary>
        public string DefaultEditor
        {
            get => _defaultEditor;
            set => SetProperty(ref _defaultEditor, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public EditorViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _defaultEditor = _config.DefaultEditor;
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.DefaultEditor = _defaultEditor;
        }
    }
}