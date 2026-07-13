using GitLocalService.Models;
using GitLocalService.Services;
using Microsoft.Win32;
using Prism.Mvvm;
using System.Collections.Generic;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 选择组件界面视图模型
    /// <para>显示可选组件列表，支持树形结构（包含子组件）</para>
    /// </summary>
    public class ComponentsViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 可选组件列表（从InstallConfig获取）
        /// <para>支持树形结构，每个组件可包含子组件</para>
        /// </summary>
        public List<ComponentItem> Components => _config.Components;

        private bool _checked;

        public bool Checked
        {
            get => _checked;
            set => SetProperty(ref _checked, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public ComponentsViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;

            Checked = IsGitBashInContextMenu();
        }

        public bool IsGitBashInContextMenu()
        {
            // 检查常见注册表路径
            string[] paths = new[]
            {
                @"Directory\Background\shell\git_shell",
                @"Directory\shell\git_shell",
                @"Directory\Background\shell\git",
                @"Directory\shell\git"
            };

            // 先查 HKEY_CLASSES_ROOT
            foreach (var path in paths)
            {
                using var key = Registry.ClassesRoot.OpenSubKey(path);
                if (key != null)
                    return true;
            }

            // 再查 HKEY_CURRENT_USER
            foreach (var path in paths)
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\" + path);
                if (key != null)
                    return true;
            }

            return false;
        }
    }
}