using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;
using System.Text;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 安装摘要界面视图模型
    /// <para>显示用户在安装向导中选择的所有配置选项</para>
    /// </summary>
    public class ReadyViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：安装摘要文本
        /// </summary>
        private string _summaryText;

        /// <summary>
        /// 安装摘要文本
        /// <para>包含所有用户选择的配置选项的汇总信息</para>
        /// </summary>
        public string SummaryText
        {
            get => _summaryText;
            set => SetProperty(ref _summaryText, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public ReadyViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            GenerateSummary();
        }

        /// <summary>
        /// 生成安装摘要文本
        /// <para>将所有配置选项格式化为可读的文本格式</para>
        /// </summary>
        private void GenerateSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Destination Location: {_config.InstallPath}");
            sb.AppendLine($"Start Menu Folder: {_config.StartMenuFolder}");
            sb.AppendLine();
            sb.AppendLine("Components:");
            
            foreach (var component in _config.Components)
            {
                if (component.Checked)
                {
                    sb.AppendLine($"  • {component.DisplayName}");
                    foreach (var child in component.Children)
                    {
                        if (child.Checked)
                        {
                            sb.AppendLine($"    - {child.DisplayName}");
                        }
                    }
                }
            }
            
            sb.AppendLine();
            sb.AppendLine($"Default Editor: {_config.DefaultEditor}");
            
            if (_config.UseCustomBranchName)
            {
                sb.AppendLine($"Initial Branch Name: {_config.InitialBranchName}");
            }
            else
            {
                sb.AppendLine("Initial Branch Name: Let Git decide (master)");
            }
            
            sb.AppendLine($"PATH Environment: {GetPathOptionDisplay(_config.PathOption)}");
            sb.AppendLine($"SSH Executable: {GetSshOptionDisplay(_config.SshOption)}");
            sb.AppendLine($"HTTPS Backend: {GetHttpsOptionDisplay(_config.HttpsBackend)}");
            sb.AppendLine($"Line Ending: {GetLineEndingDisplay(_config.LineEndingOption)}");
            sb.AppendLine($"Terminal: {GetTerminalDisplay(_config.TerminalOption)}");
            sb.AppendLine($"Pull Behavior: {GetPullDisplay(_config.PullBehavior)}");
            
            sb.AppendLine();
            sb.AppendLine("Extra Options:");
            if (_config.EnableFileSystemCaching)
                sb.AppendLine("  • Enable file system caching");
            if (_config.EnableSymbolicLinks)
                sb.AppendLine("  • Enable symbolic links");
            if (_config.EnablePseudoConsole)
                sb.AppendLine("  • Enable pseudo console support");

            SummaryText = sb.ToString();
        }

        /// <summary>
        /// 获取PATH选项的显示文本
        /// </summary>
        /// <param name="option">PATH选项值</param>
        /// <returns>用户友好的显示文本</returns>
        private string GetPathOptionDisplay(string option)
        {
            switch (option)
            {
                case "GitBashOnly": return "Git Bash only";
                case "CmdAndThirdParty": return "Git from command line and 3rd-party software";
                case "CmdAndUnixTools": return "Git and Unix tools from Command Prompt";
                default: return option;
            }
        }

        /// <summary>
        /// 获取SSH选项的显示文本
        /// </summary>
        /// <param name="option">SSH选项值</param>
        /// <returns>用户友好的显示文本</returns>
        private string GetSshOptionDisplay(string option)
        {
            switch (option)
            {
                case "BundledOpenSSH": return "Use bundled OpenSSH";
                case "ExternalOpenSSH": return "Use external OpenSSH";
                default: return option;
            }
        }

        /// <summary>
        /// 获取HTTPS选项的显示文本
        /// </summary>
        /// <param name="option">HTTPS选项值</param>
        /// <returns>用户友好的显示文本</returns>
        private string GetHttpsOptionDisplay(string option)
        {
            switch (option)
            {
                case "OpenSSL": return "Use the OpenSSL library";
                case "SecureChannel": return "Use native Windows Secure Channel";
                default: return option;
            }
        }

        /// <summary>
        /// 获取换行符选项的显示文本
        /// </summary>
        /// <param name="option">换行符选项值</param>
        /// <returns>用户友好的显示文本</returns>
        private string GetLineEndingDisplay(string option)
        {
            switch (option)
            {
                case "WindowsCheckoutUnixCommit": return "Checkout Windows-style, commit Unix-style";
                case "AsIs": return "Checkout as-is, commit as-is";
                case "Unix": return "Checkout Unix-style, commit Unix-style";
                default: return option;
            }
        }

        /// <summary>
        /// 获取终端选项的显示文本
        /// </summary>
        /// <param name="option">终端选项值</param>
        /// <returns>用户友好的显示文本</returns>
        private string GetTerminalDisplay(string option)
        {
            switch (option)
            {
                case "MinTTY": return "Use MinTTY";
                case "WindowsConsole": return "Use Windows' default console";
                default: return option;
            }
        }

        /// <summary>
        /// 获取拉取行为选项的显示文本
        /// </summary>
        /// <param name="option">拉取行为选项值</param>
        /// <returns>用户友好的显示文本</returns>
        private string GetPullDisplay(string option)
        {
            switch (option)
            {
                case "Merge": return "Merge (fast-forward if possible)";
                case "Rebase": return "Rebase";
                case "FastForwardOnly": return "Fast-forward only";
                default: return option;
            }
        }
    }
}