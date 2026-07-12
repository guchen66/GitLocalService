using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.IO;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 选择安装路径界面视图模型
    /// <para>允许用户指定Git的安装目录，并显示磁盘可用空间</para>
    /// </summary>
    public class DestinationViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：安装路径
        /// </summary>
        private string _installPath;

        /// <summary>
        /// 私有字段：可用空间信息
        /// </summary>
        private string _availableSpace;

        /// <summary>
        /// 安装路径
        /// <para>绑定到文本框控件</para>
        /// </summary>
        public string InstallPath
        {
            get => _installPath;
            set => SetProperty(ref _installPath, value);
        }

        /// <summary>
        /// 可用空间信息
        /// <para>格式："Available: X GB"</para>
        /// </summary>
        public string AvailableSpace
        {
            get => _availableSpace;
            set => SetProperty(ref _availableSpace, value);
        }

        /// <summary>
        /// 浏览命令，打开文件夹选择对话框
        /// </summary>
        public DelegateCommand BrowseCommand { get; }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public DestinationViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _installPath = _config.InstallPath;
            BrowseCommand = new DelegateCommand(Browse);
            UpdateAvailableSpace();
        }

        /// <summary>
        /// 浏览文件夹
        /// <para>打开Windows文件夹选择对话框，允许用户选择安装目录</para>
        /// </summary>
        private void Browse()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = InstallPath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InstallPath = dialog.SelectedPath;
                UpdateAvailableSpace();
            }
        }

        /// <summary>
        /// 更新可用空间信息
        /// <para>根据安装路径获取磁盘剩余空间</para>
        /// </summary>
        private void UpdateAvailableSpace()
        {
            try
            {
                var drive = new DriveInfo(Path.GetPathRoot(InstallPath));
                AvailableSpace = $"Available: {FormatSize(drive.AvailableFreeSpace)}";
            }
            catch
            {
                AvailableSpace = "Available: Unknown";
            }
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="bytes">字节数</param>
        /// <returns>格式化后的大小字符串（GB/MB/KB/bytes）</returns>
        private string FormatSize(long bytes)
        {
            if (bytes >= 1024 * 1024 * 1024)
                return $"{bytes / (1024 * 1024 * 1024):F1} GB";
            if (bytes >= 1024 * 1024)
                return $"{bytes / (1024 * 1024):F1} MB";
            if (bytes >= 1024)
                return $"{bytes / 1024:F1} KB";
            return $"{bytes} bytes";
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.InstallPath = _installPath;
        }
    }
}