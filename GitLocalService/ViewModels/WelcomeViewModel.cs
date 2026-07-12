using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 欢迎界面视图模型
    /// <para>显示安装向导的欢迎信息、版本号和安装大小</para>
    /// <para>支持检测本地已安装的Git并显示用户配置</para>
    /// </summary>
    public class WelcomeViewModel : BindableBase
    {
        /// <summary>
        /// 私有字段：Git检测结果
        /// </summary>
        private GitDetectResult _detectResult;

        /// <summary>
        /// 私有字段：用户输入的姓名
        /// </summary>
        private string _userName;

        /// <summary>
        /// 私有字段：用户输入的邮箱
        /// </summary>
        private string _userEmail;

        /// <summary>
        /// 向导服务，用于访问安装配置
        /// </summary>
        private readonly IWizardService _wizardService;

        /// <summary>
        /// Git检测结果
        /// <para>包含是否找到Git、版本、安装大小、用户配置等信息</para>
        /// </summary>
        public GitDetectResult DetectResult
        {
            get => _detectResult;
            set => SetProperty(ref _detectResult, value);
        }

        /// <summary>
        /// 用户输入的姓名
        /// <para>绑定到Expander中的TextBox</para>
        /// <para>同时同步到InstallConfig中</para>
        /// </summary>
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
                // 同步到安装配置
                if (_wizardService != null)
                    _wizardService.Config.UserName = value;
            }
        }

        /// <summary>
        /// 用户输入的邮箱
        /// <para>绑定到Expander中的TextBox</para>
        /// <para>同时同步到InstallConfig中</para>
        /// </summary>
        public string UserEmail
        {
            get => _userEmail;
            set
            {
                SetProperty(ref _userEmail, value);
                // 同步到安装配置
                if (_wizardService != null)
                    _wizardService.Config.UserEmail = value;
            }
        }

        /// <summary>
        /// Git版本号（显示用）
        /// <para>如果未检测到Git，显示默认值</para>
        /// </summary>
        public string VersionDisplay
        {
            get
            {
                if (DetectResult?.IsFound ?? false)
                    return DetectResult.VersionNumber;
                return "2.45.1.windows.1";
            }
        }

        /// <summary>
        /// 安装大小（显示用）
        /// <para>如果未检测到Git或无法计算大小，显示默认值</para>
        /// </summary>
        public string SizeDisplay
        {
            get
            {
                if (DetectResult?.IsFound ?? false && DetectResult.InstallSize > 0)
                    return $"~ {DetectResult.FormattedSize}";
                return "~ 300 MB";
            }
        }

        /// <summary>
        /// 是否检测到Git
        /// </summary>
        public bool GitFound => DetectResult?.IsFound ?? false;

        /// <summary>
        /// 构造函数
        /// <para>初始化时自动检测本地Git安装情况</para>
        /// </summary>
        /// <param name="wizardService">向导服务，用于访问安装配置</param>
        public WelcomeViewModel(IWizardService wizardService)
        {
            _wizardService = wizardService;
            DetectGit();
        }

        /// <summary>
        /// 构造函数（无参数版本，用于设计时支持）
        /// </summary>
        public WelcomeViewModel()
        {
            DetectGit();
        }

        /// <summary>
        /// 检测本地Git安装情况
        /// <para>调用GitDetectorService进行检测，并初始化用户配置字段</para>
        /// </summary>
        private void DetectGit()
        {
            DetectResult = GitDetectorService.Detect();
            
            // 初始化用户配置字段
            if (DetectResult?.IsFound ?? false)
            {
                UserName = DetectResult.UserName ?? string.Empty;
                UserEmail = DetectResult.UserEmail ?? string.Empty;
            }
        }

        /// <summary>
        /// 保存用户配置到Git
        /// <para>使用GitDetectorService设置user.name和user.email</para>
        /// </summary>
        /// <returns>是否保存成功</returns>
        public bool SaveUserConfig()
        {
            if (!GitFound || string.IsNullOrEmpty(DetectResult.GitPath))
                return false;

            bool nameSaved = string.IsNullOrEmpty(UserName) || 
                            GitDetectorService.SetGitConfig(DetectResult.GitPath, "user.name", UserName);
            bool emailSaved = string.IsNullOrEmpty(UserEmail) || 
                            GitDetectorService.SetGitConfig(DetectResult.GitPath, "user.email", UserEmail);

            return nameSaved && emailSaved;
        }
    }
}