using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Mvvm;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// HTTPS传输后端选择界面视图模型
    /// <para>允许用户选择Git使用的HTTPS加密库</para>
    /// </summary>
    public class HttpsViewModel : BindableBase
    {
        /// <summary>
        /// 安装配置对象
        /// </summary>
        private readonly InstallConfig _config;

        /// <summary>
        /// 私有字段：HTTPS传输后端
        /// </summary>
        private string _httpsBackend;

        /// <summary>
        /// HTTPS传输后端
        /// <para>可选值：</para>
        /// <para>- OpenSSL：使用OpenSSL库</para>
        /// <para>- SecureChannel：使用Windows原生Secure Channel</para>
        /// <para>默认值："OpenSSL"</para>
        /// </summary>
        public string HttpsBackend
        {
            get => _httpsBackend;
            set => SetProperty(ref _httpsBackend, value);
        }

        /// <summary>
        /// 构造函数，注入向导服务
        /// </summary>
        /// <param name="wizardService">向导服务</param>
        public HttpsViewModel(IWizardService wizardService)
        {
            _config = wizardService.Config;
            _httpsBackend = _config.HttpsBackend;
        }

        /// <summary>
        /// 保存配置到InstallConfig对象
        /// <para>在用户点击"下一步"时调用</para>
        /// </summary>
        public void SaveConfig()
        {
            _config.HttpsBackend = _httpsBackend;
        }
    }
}