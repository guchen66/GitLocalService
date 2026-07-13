using System.Collections.Generic;

namespace GitLocalService.Models
{
    /// <summary>
    /// 安装配置类，存储所有安装向导步骤中用户选择的配置选项
    /// </summary>
    public class InstallConfig
    {
        /// <summary>
        /// 私有字段：用户姓名，用于设置 git config --global user.name
        /// </summary>
        private string _userName;

        /// <summary>
        /// 用户姓名
        /// <para>在欢迎界面的Expander中输入，用于设置 git config --global user.name</para>
        /// </summary>
        public string UserName
        {
            get => _userName;
            set => _userName = value;
        }

        /// <summary>
        /// 私有字段：用户邮箱，用于设置 git config --global user.email
        /// </summary>
        private string _userEmail;

        /// <summary>
        /// 用户邮箱
        /// <para>在欢迎界面的Expander中输入，用于设置 git config --global user.email</para>
        /// </summary>
        public string UserEmail
        {
            get => _userEmail;
            set => _userEmail = value;
        }

        /// <summary>
        /// 私有字段：安装路径，默认值为"C:\\Program Files\\Git"
        /// </summary>
        private string _installPath = @"C:\Program Files\Git";

        /// <summary>
        /// Git的安装路径
        /// <para>用户在"选择目标位置"页面中指定的安装目录</para>
        /// <para>默认值：C:\Program Files\Git</para>
        /// </summary>
        public string InstallPath
        {
            get => _installPath;
            set => _installPath = value;
        }

        /// <summary>
        /// 私有字段：开始菜单文件夹名称，默认值为"Git"
        /// </summary>
        private string _startMenuFolder = "Git";

        /// <summary>
        /// 开始菜单文件夹名称
        /// <para>用户在"选择开始菜单文件夹"页面中指定的文件夹名称</para>
        /// <para>默认值：Git</para>
        /// </summary>
        public string StartMenuFolder
        {
            get => _startMenuFolder;
            set => _startMenuFolder = value;
        }

        /// <summary>
        /// 私有字段：默认编辑器，默认值为"Vim"
        /// </summary>
        private string _defaultEditor = "Vim";

        /// <summary>
        /// Git使用的默认文本编辑器
        /// <para>用户在"选择默认编辑器"页面中选择的编辑器</para>
        /// <para>可选值：Vim, Nano, Notepad, Notepad++, VS Code, Visual Studio等</para>
        /// <para>默认值：Vim</para>
        /// </summary>
        public string DefaultEditor
        {
            get => _defaultEditor;
            set => _defaultEditor = value;
        }

        /// <summary>
        /// 私有字段：初始分支名称，默认值为"master"
        /// </summary>
        private string _initialBranchName = "master";

        /// <summary>
        /// Git仓库的初始分支名称
        /// <para>当UseCustomBranchName为true时生效</para>
        /// <para>默认值：master</para>
        /// </summary>
        public string InitialBranchName
        {
            get => _initialBranchName;
            set => _initialBranchName = value;
        }

        /// <summary>
        /// 私有字段：是否使用自定义分支名称，默认值为false
        /// </summary>
        private bool _useCustomBranchName = false;

        /// <summary>
        /// 是否使用自定义分支名称
        /// <para>为true时，使用InitialBranchName指定的名称作为初始分支名</para>
        /// <para>为false时，使用Git默认行为（通常为master）</para>
        /// <para>默认值：false</para>
        /// </summary>
        public bool UseCustomBranchName
        {
            get => _useCustomBranchName;
            set => _useCustomBranchName = value;
        }

        /// <summary>
        /// 私有字段：PATH环境变量配置选项，默认值为"CmdAndThirdParty"
        /// </summary>
        private string _pathOption = "CmdAndThirdParty";

        /// <summary>
        /// PATH环境变量配置选项
        /// <para>用户在"调整PATH环境变量"页面中选择的选项</para>
        /// <para>可选值：</para>
        /// <para>- GitBashOnly：仅在Git Bash中使用Git</para>
        /// <para>- CmdAndThirdParty：在命令行和第三方软件中使用Git（推荐）</para>
        /// <para>- CmdAndUnixTools：在命令行中使用Git和Unix工具</para>
        /// <para>默认值：CmdAndThirdParty</para>
        /// </summary>
        public string PathOption
        {
            get => _pathOption;
            set => _pathOption = value;
        }

        /// <summary>
        /// 私有字段：SSH可执行文件选项，默认值为"BundledOpenSSH"
        /// </summary>
        private string _sshOption = "BundledOpenSSH";

        /// <summary>
        /// SSH可执行文件选项
        /// <para>用户在"选择SSH可执行文件"页面中选择的选项</para>
        /// <para>可选值：</para>
        /// <para>- BundledOpenSSH：使用Git自带的OpenSSH</para>
        /// <para>- ExternalOpenSSH：使用外部安装的OpenSSH</para>
        /// <para>默认值：BundledOpenSSH</para>
        /// </summary>
        public string SshOption
        {
            get => _sshOption;
            set => _sshOption = value;
        }

        /// <summary>
        /// 私有字段：HTTPS传输后端，默认值为"OpenSSL"
        /// </summary>
        private string _httpsBackend = "OpenSSL";

        /// <summary>
        /// HTTPS传输后端
        /// <para>用户在"选择HTTPS传输后端"页面中选择的选项</para>
        /// <para>可选值：</para>
        /// <para>- OpenSSL：使用OpenSSL库</para>
        /// <para>- SecureChannel：使用Windows原生Secure Channel</para>
        /// <para>默认值：OpenSSL</para>
        /// </summary>
        public string HttpsBackend
        {
            get => _httpsBackend;
            set => _httpsBackend = value;
        }

        /// <summary>
        /// 私有字段：换行符转换选项，默认值为"WindowsCheckoutUnixCommit"
        /// </summary>
        private string _lineEndingOption = "WindowsCheckoutUnixCommit";

        /// <summary>
        /// 换行符转换选项
        /// <para>用户在"配置换行符转换"页面中选择的选项</para>
        /// <para>可选值：</para>
        /// <para>- WindowsCheckoutUnixCommit：检出时转换为Windows格式，提交时转换为Unix格式（推荐）</para>
        /// <para>- AsIs：保持原样，检出和提交都不转换</para>
        /// <para>- Unix：检出和提交都使用Unix格式</para>
        /// <para>默认值：WindowsCheckoutUnixCommit</para>
        /// </summary>
        public string LineEndingOption
        {
            get => _lineEndingOption;
            set => _lineEndingOption = value;
        }

        /// <summary>
        /// 私有字段：终端模拟器选项，默认值为"MinTTY"
        /// </summary>
        private string _terminalOption = "MinTTY";

        /// <summary>
        /// 终端模拟器选项
        /// <para>用户在"配置终端模拟器"页面中选择的选项</para>
        /// <para>可选值：</para>
        /// <para>- MinTTY：使用MinTTY（Git Bash默认终端）</para>
        /// <para>- WindowsConsole：使用Windows默认控制台</para>
        /// <para>默认值：MinTTY</para>
        /// </summary>
        public string TerminalOption
        {
            get => _terminalOption;
            set => _terminalOption = value;
        }

        /// <summary>
        /// 私有字段：默认拉取行为，默认值为"Merge"
        /// </summary>
        private string _pullBehavior = "Merge";

        /// <summary>
        /// 默认拉取行为
        /// <para>用户在"配置git pull默认行为"页面中选择的选项</para>
        /// <para>可选值：</para>
        /// <para>- Merge：合并（如果可能则快进）</para>
        /// <para>- Rebase：变基</para>
        /// <para>- FastForwardOnly：仅快进</para>
        /// <para>默认值：Merge</para>
        /// </summary>
        public string PullBehavior
        {
            get => _pullBehavior;
            set => _pullBehavior = value;
        }

        /// <summary>
        /// 私有字段：是否启用文件系统缓存，默认值为true
        /// </summary>
        private bool _enableFileSystemCaching = true;

        /// <summary>
        /// 是否启用文件系统缓存
        /// <para>启用后可提高Git操作性能</para>
        /// <para>默认值：true</para>
        /// </summary>
        public bool EnableFileSystemCaching
        {
            get => _enableFileSystemCaching;
            set => _enableFileSystemCaching = value;
        }

        /// <summary>
        /// 私有字段：是否启用符号链接，默认值为true
        /// </summary>
        private bool _enableSymbolicLinks = true;

        /// <summary>
        /// 是否启用符号链接支持
        /// <para>启用后Git可以创建和处理符号链接</para>
        /// <para>需要管理员权限</para>
        /// <para>默认值：true</para>
        /// </summary>
        public bool EnableSymbolicLinks
        {
            get => _enableSymbolicLinks;
            set => _enableSymbolicLinks = value;
        }

        /// <summary>
        /// 私有字段：是否启用伪控制台支持，默认值为true
        /// </summary>
        private bool _enablePseudoConsole = true;

        /// <summary>
        /// 是否启用伪控制台支持
        /// <para>在Windows 10 1809+上提供更好的终端体验</para>
        /// <para>默认值：true</para>
        /// </summary>
        public bool EnablePseudoConsole
        {
            get => _enablePseudoConsole;
            set => _enablePseudoConsole = value;
        }

        /// <summary>
        /// 私有字段：是否启动Git Bash，默认值为true
        /// </summary>
        private bool _launchGitBash = true;

        /// <summary>
        /// 是否在安装完成后启动Git Bash
        /// <para>默认值：true</para>
        /// </summary>
        public bool LaunchGitBash
        {
            get => _launchGitBash;
            set => _launchGitBash = value;
        }

        /// <summary>
        /// 私有字段：是否查看发行说明，默认值为true
        /// </summary>
        private bool _viewReleaseNotes = true;

        /// <summary>
        /// 是否在安装完成后查看发行说明
        /// <para>默认值：true</para>
        /// </summary>
        public bool ViewReleaseNotes
        {
            get => _viewReleaseNotes;
            set => _viewReleaseNotes = value;
        }

        /// <summary>
        /// 可选组件列表
        /// <para>存储用户在"选择组件"页面中勾选的组件选项</para>
        /// <para>默认包含以下组件：</para>
        /// <para>- AdditionalIcons（附加图标）：包含DesktopIcon（桌面图标）</para>
        /// <para>- WindowsExplorerIntegration（Windows资源管理器集成）：包含GitBashHere和GitGUIHere</para>
        /// <para>- GitLFS（Git Large File Storage）</para>
        /// <para>- AssociateGitFiles（关联.git文件）</para>
        /// <para>- AssociateShFiles（关联.sh文件）</para>
        /// <para>- CheckDailyUpdates（每日检查更新）- 默认未勾选</para>
        /// <para>- AddGitBashToTerminal（添加Git Bash到Terminal）- 默认未勾选</para>
        /// </summary>
        public List<ComponentItem> Components { get; set; } = new List<ComponentItem>
        {
            new ComponentItem { Name = "AdditionalIcons", DisplayName = "Additional icons", Checked = true, Children = new List<ComponentItem>
                {
                    new ComponentItem { Name = "DesktopIcon", DisplayName = "On the Desktop", Checked = true }
                }
            },
            new ComponentItem { Name = "WindowsExplorerIntegration", DisplayName = "Windows Explorer integration", Checked = true, Children = new List<ComponentItem>
                {
                    new ComponentItem { Name = "GitBashHere", DisplayName = "Git Bash Here", Checked = true },
                    new ComponentItem { Name = "GitGUIHere", DisplayName = "Git GUI Here", Checked = true }
                }
            },
            new ComponentItem { Name = "GitLFS", DisplayName = "Git LFS", Checked = true },
            new ComponentItem { Name = "AssociateGitFiles", DisplayName = "Associate .git files", Checked = true },
            new ComponentItem { Name = "AssociateShFiles", DisplayName = "Associate .sh files", Checked = true },
            new ComponentItem { Name = "CheckDailyUpdates", DisplayName = "Check daily for updates", Checked = false },
            new ComponentItem { Name = "AddGitBashToTerminal", DisplayName = "Add Git Bash to Terminal", Checked = false }
        };
    }

    /// <summary>
    /// 组件项类，用于表示安装向导中的可选组件
    /// <para>支持树形结构，可包含子组件</para>
    /// </summary>
    public class ComponentItem
    {
        /// <summary>
        /// 组件的内部名称（用于程序逻辑识别）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组件的显示名称（用于UI展示）
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否勾选该组件
        /// <para>true表示用户选择安装该组件，false表示不安装</para>
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// 子组件列表
        /// <para>用于表示组件的层级关系，如"Windows资源管理器集成"包含"Git Bash Here"和"Git GUI Here"</para>
        /// </summary>
        public List<ComponentItem> Children { get; set; } = new List<ComponentItem>();
    }
}