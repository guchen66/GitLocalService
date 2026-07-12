namespace GitLocalService.Models
{
    /// <summary>
    /// Git检测结果类，用于存储检测本地已安装Git的结果信息
    /// </summary>
    public class GitDetectResult
    {
        /// <summary>
        /// 是否检测到Git
        /// <para>true表示检测到已安装的Git，false表示未检测到</para>
        /// </summary>
        public bool IsFound { get; set; }

        /// <summary>
        /// Git可执行文件的路径
        /// <para>当IsFound为true时有效</para>
        /// <para>可能的值：</para>
        /// <para>- "git"（通过环境变量检测到）</para>
        /// <para>- 完整路径（如 "C:\Program Files\Git\cmd\git.exe"）</para>
        /// </summary>
        public string GitPath { get; set; }

        /// <summary>
        /// Git版本信息
        /// <para>当IsFound为true时有效</para>
        /// <para>格式：git version x.y.z.w</para>
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 检测来源
        /// <para>描述从哪个位置检测到Git</para>
        /// <para>可能的值：</para>
        /// <para>- "环境变量"：从系统PATH环境变量中检测到</para>
        /// <para>- "常见安装路径"：从常见的安装目录中检测到</para>
        /// <para>- "注册表"：从Windows注册表中检测到</para>
        /// <para>- "未找到Git"：未检测到任何Git安装</para>
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Git配置的用户姓名
        /// <para>通过 git config user.name 获取</para>
        /// <para>如果未配置则返回空字符串</para>
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Git配置的用户邮箱
        /// <para>通过 git config user.email 获取</para>
        /// <para>如果未配置则返回空字符串</para>
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 安装大小（字节）
        /// <para>通过计算安装目录的总大小获得</para>
        /// <para>如果无法计算则返回0</para>
        /// </summary>
        public long InstallSize { get; set; }

        /// <summary>
        /// 格式化后的安装大小字符串
        /// <para>根据InstallSize自动转换为KB/MB/GB格式</para>
        /// </summary>
        public string FormattedSize
        {
            get
            {
                if (InstallSize >= 1024 * 1024 * 1024)
                    return $"{InstallSize / (1024.0 * 1024 * 1024):F1} GB";
                if (InstallSize >= 1024 * 1024)
                    return $"{InstallSize / (1024.0 * 1024):F1} MB";
                if (InstallSize >= 1024)
                    return $"{InstallSize / 1024.0:F1} KB";
                return $"{InstallSize} bytes";
            }
        }

        /// <summary>
        /// 纯版本号（去除 "git version " 前缀）
        /// <para>例如："2.45.1.windows.1"</para>
        /// </summary>
        public string VersionNumber
        {
            get
            {
                if (string.IsNullOrEmpty(Version))
                    return "Unknown";
                if (Version.StartsWith("git version "))
                    return Version.Substring(12);
                return Version;
            }
        }
    }
}