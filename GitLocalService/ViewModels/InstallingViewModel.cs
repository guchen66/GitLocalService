using GitLocalService.Services;
using Prism.Mvvm;
using System.IO;
using System.Threading;
using System.Windows;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 安装进度界面视图模型
    /// <para>模拟安装过程，显示安装进度和当前操作</para>
    /// <para>安装完成后自动设置git config --global user.name和user.email</para>
    /// </summary>
    public class InstallingViewModel : BindableBase
    {
        /// <summary>
        /// 私有字段：安装进度（0-100）
        /// </summary>
        private int _progress = 0;

        /// <summary>
        /// 私有字段：当前安装操作描述
        /// </summary>
        private string _currentAction = "Preparing installation...";

        /// <summary>
        /// 向导服务，用于获取安装配置
        /// </summary>
        private readonly IWizardService _wizardService;

        /// <summary>
        /// 安装进度（0-100）
        /// <para>绑定到进度条控件</para>
        /// </summary>
        public int Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        /// <summary>
        /// 当前安装操作描述
        /// <para>显示正在执行的安装步骤</para>
        /// </summary>
        public string CurrentAction
        {
            get => _currentAction;
            set => SetProperty(ref _currentAction, value);
        }

        /// <summary>
        /// 构造函数
        /// <para>初始化时自动开始模拟安装过程</para>
        /// </summary>
        /// <param name="wizardService">向导服务，用于获取安装配置</param>
        public InstallingViewModel(IWizardService wizardService)
        {
            _wizardService = wizardService;
            StartInstallation();
        }

        /// <summary>
        /// 构造函数（无参数版本，用于设计时支持）
        /// </summary>
        public InstallingViewModel()
        {
            StartInstallation();
        }

        /// <summary>
        /// 开始安装过程
        /// <para>在后台线程中执行，避免阻塞UI</para>
        /// </summary>
        private void StartInstallation()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                SimulateInstallation();
            });
        }

        /// <summary>
        /// 模拟安装过程
        /// <para>按顺序执行多个安装步骤，每步更新进度</para>
        /// <para>完成后通知主窗口切换到完成界面</para>
        /// <para>最后设置git config --global user.name和user.email</para>
        /// </summary>
        private void SimulateInstallation()
        {
            Thread.Sleep(500);
            
            UpdateProgress(5, "Extracting files...");
            Thread.Sleep(300);
            
            UpdateProgress(15, "Copying git.exe...");
            Thread.Sleep(300);
            
            UpdateProgress(25, "Copying git-cmd.exe...");
            Thread.Sleep(300);
            
            UpdateProgress(35, "Copying Git Bash...");
            Thread.Sleep(300);
            
            UpdateProgress(45, "Installing documentation...");
            Thread.Sleep(400);
            
            UpdateProgress(55, "Configuring PATH...");
            Thread.Sleep(300);
            
            UpdateProgress(65, "Registering git.exe...");
            Thread.Sleep(300);
            
            UpdateProgress(75, "Creating Start Menu shortcuts...");
            Thread.Sleep(300);
            
            UpdateProgress(85, "Creating desktop shortcuts...");
            Thread.Sleep(300);
            
            UpdateProgress(95, "Finalizing installation...");
            Thread.Sleep(500);
            
            // 设置用户配置（git config --global）
            UpdateProgress(98, "Configuring user settings...");
            ConfigureUserSettings();
            
            UpdateProgress(100, "Installation complete!");
            
            // 安装完成后通知主窗口更新状态
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = Application.Current.MainWindow;
                var mainViewModel = (MainWindowViewModel)mainWindow.DataContext;
                mainViewModel.UpdateProgress(100, "Installation complete!");
            });
        }

        /// <summary>
        /// 配置用户设置（git config --global）
        /// <para>如果用户在欢迎界面输入了姓名和邮箱，则设置全局git配置</para>
        /// </summary>
        private void ConfigureUserSettings()
        {
            try
            {
                if (_wizardService == null)
                    return;

                var config = _wizardService.Config;
                
                // 如果用户输入了姓名或邮箱，才进行配置
                if (string.IsNullOrEmpty(config.UserName) && string.IsNullOrEmpty(config.UserEmail))
                    return;

                // 构建git.exe路径（安装完成后应该在该路径）
                string gitExePath = Path.Combine(config.InstallPath, "cmd", "git.exe");
                
                // 如果路径不存在，尝试其他常见路径
                if (!File.Exists(gitExePath))
                {
                    gitExePath = Path.Combine(config.InstallPath, "bin", "git.exe");
                }
                
                // 如果仍然不存在，尝试直接使用"git"（可能已在PATH中）
                if (!File.Exists(gitExePath))
                {
                    gitExePath = "git";
                }

                // 设置user.name
                if (!string.IsNullOrEmpty(config.UserName))
                {
                    GitDetectorService.SetGitConfig(gitExePath, "user.name", config.UserName);
                }

                // 设置user.email
                if (!string.IsNullOrEmpty(config.UserEmail))
                {
                    GitDetectorService.SetGitConfig(gitExePath, "user.email", config.UserEmail);
                }
            }
            catch
            {
                // 配置失败不影响安装流程
            }
        }

        /// <summary>
        /// 更新安装进度
        /// <para>在UI线程中更新进度值和操作描述</para>
        /// </summary>
        /// <param name="progress">进度值（0-100）</param>
        /// <param name="action">当前操作描述</param>
        private void UpdateProgress(int progress, string action)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Progress = progress;
                CurrentAction = action;
            });
            
            Thread.Sleep(100);
        }
    }
}