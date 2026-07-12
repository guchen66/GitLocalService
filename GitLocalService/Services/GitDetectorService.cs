using GitLocalService.Models;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

namespace GitLocalService.Services
{
    /// <summary>
    /// Git检测服务类，用于检测本地是否已安装Git
    /// <para>支持多种检测方式：环境变量、常见安装路径、注册表</para>
    /// </summary>
    public static class GitDetectorService
    {
        /// <summary>
        /// 检测本地Git安装情况
        /// <para>按以下顺序进行检测：</para>
        /// <para>1. 从环境变量检测（通过运行 git --version 命令）</para>
        /// <para>2. 从常见安装路径检测</para>
        /// <para>3. 从注册表检测（Git for Windows安装记录）</para>
        /// </summary>
        /// <returns>Git检测结果对象，包含是否找到、路径、版本和来源信息</returns>
        public static GitDetectResult Detect()
        {
            GitDetectResult result;

            // 优先从环境变量检测
            result = DetectFromEnvironment();
            if (result.IsFound)
            {
                CompleteResult(result);
                return result;
            }

            // 其次从常见安装路径检测
            result = DetectFromCommonPaths();
            if (result.IsFound)
            {
                CompleteResult(result);
                return result;
            }

            // 最后从注册表检测
            result = DetectFromRegistry();
            if (result.IsFound)
            {
                CompleteResult(result);
                return result;
            }

            // 未找到Git
            return new GitDetectResult
            {
                IsFound = false,
                Source = "未找到Git"
            };
        }

        /// <summary>
        /// 完成检测结果，补充用户配置和安装大小信息
        /// </summary>
        /// <param name="result">已检测到Git的结果对象</param>
        private static void CompleteResult(GitDetectResult result)
        {
            // 获取用户配置
            result.UserName = GetGitConfig(result.GitPath, "user.name");
            result.UserEmail = GetGitConfig(result.GitPath, "user.email");

            // 计算安装大小
            result.InstallSize = CalculateInstallSize(result.GitPath);
        }

        /// <summary>
        /// 获取Git配置值
        /// </summary>
        /// <param name="gitPath">Git可执行文件路径</param>
        /// <param name="configKey">配置键名（如 "user.name"、"user.email"）</param>
        /// <returns>配置值，如果配置不存在或获取失败则返回空字符串</returns>
        public static string GetGitConfig(string gitPath, string configKey)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = gitPath,
                    Arguments = $"config --get {configKey}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit(3000);

                return process.ExitCode == 0 ? output : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设置Git配置值
        /// </summary>
        /// <param name="gitPath">Git可执行文件路径</param>
        /// <param name="configKey">配置键名</param>
        /// <param name="configValue">配置值</param>
        /// <returns>是否设置成功</returns>
        public static bool SetGitConfig(string gitPath, string configKey, string configValue)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = gitPath,
                    Arguments = $"config --global {configKey} \"{configValue}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                process.WaitForExit(3000);

                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算Git安装目录的大小
        /// </summary>
        /// <param name="gitPath">Git可执行文件路径</param>
        /// <returns>安装目录的总大小（字节），计算失败返回0</returns>
        private static long CalculateInstallSize(string gitPath)
        {
            try
            {
                // 获取Git安装根目录
                string installRoot;
                if (gitPath.Equals("git", System.StringComparison.OrdinalIgnoreCase))
                {
                    // 通过 which 命令查找实际路径
                    installRoot = GetGitInstallRootFromWhich();
                }
                else
                {
                    // 从完整路径推导安装根目录
                    // git.exe 通常在 <installRoot>\cmd\git.exe
                    // 或 <installRoot>\bin\git.exe
                    var dirInfo = new DirectoryInfo(Path.GetDirectoryName(gitPath));
                    if (dirInfo.Name.Equals("cmd", System.StringComparison.OrdinalIgnoreCase) ||
                        dirInfo.Name.Equals("bin", System.StringComparison.OrdinalIgnoreCase))
                    {
                        installRoot = dirInfo.Parent.FullName;
                    }
                    else
                    {
                        installRoot = dirInfo.FullName;
                    }
                }

                if (!string.IsNullOrEmpty(installRoot) && Directory.Exists(installRoot))
                {
                    return GetDirectorySize(installRoot);
                }
            }
            catch { }

            return 0;
        }

        /// <summary>
        /// 通过 git --exec-path 或 which 命令获取安装根目录
        /// </summary>
        /// <returns>Git安装根目录路径，获取失败返回空字符串</returns>
        private static string GetGitInstallRootFromWhich()
        {
            try
            {
                // 先尝试 git --exec-path
                var psi = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "--exec-path",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                string execPath = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit(3000);

                if (!string.IsNullOrEmpty(execPath) && Directory.Exists(execPath))
                {
                    // exec-path 通常是 <installRoot>\libexec\git-core
                    var dirInfo = new DirectoryInfo(execPath);
                    if (dirInfo.Name.Equals("git-core", System.StringComparison.OrdinalIgnoreCase))
                    {
                        dirInfo = dirInfo.Parent; // libexec
                    }
                    if (dirInfo.Name.Equals("libexec", System.StringComparison.OrdinalIgnoreCase))
                    {
                        dirInfo = dirInfo.Parent; // installRoot
                    }
                    return dirInfo.FullName;
                }
            }
            catch { }

            return string.Empty;
        }

        /// <summary>
        /// 计算目录大小（递归）
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>目录总大小（字节）</returns>
        private static long GetDirectorySize(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                long size = 0;

                foreach (FileInfo file in dir.GetFiles())
                {
                    try { size += file.Length; } catch { }
                }

                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    try { size += GetDirectorySize(subDir.FullName); } catch { }
                }

                return size;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 从环境变量检测Git
        /// <para>通过运行 git --version 命令检测</para>
        /// </summary>
        /// <returns>Git检测结果对象</returns>
        private static GitDetectResult DetectFromEnvironment()
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit(3000);

                // 如果命令执行成功且输出以"git version"开头，表示检测到Git
                if (process.ExitCode == 0 && output.StartsWith("git version"))
                {
                    return new GitDetectResult
                    {
                        IsFound = true,
                        GitPath = "git",
                        Version = output,
                        Source = "环境变量"
                    };
                }
            }
            catch { }

            return new GitDetectResult { IsFound = false };
        }

        /// <summary>
        /// 从常见安装路径检测Git
        /// <para>检查多个常见的Git安装目录</para>
        /// </summary>
        /// <returns>Git检测结果对象</returns>
        private static GitDetectResult DetectFromCommonPaths()
        {
            // 常见的Git安装路径列表
            string[] commonPaths =
            {
                @"C:\Program Files\Git\cmd\git.exe",
                @"C:\Program Files (x86)\Git\cmd\git.exe",
                @"D:\Program Files\Git\cmd\git.exe",
                @"D:\Git\cmd\git.exe",
                @"E:\Program Files\Git\cmd\git.exe",
                @"E:\Git\cmd\git.exe",
            };

            foreach (var path in commonPaths)
            {
                if (File.Exists(path))
                {
                    string version = GetGitVersion(path);
                    return new GitDetectResult
                    {
                        IsFound = true,
                        GitPath = path,
                        Version = version,
                        Source = "常见安装路径"
                    };
                }
            }

            return new GitDetectResult { IsFound = false };
        }

        /// <summary>
        /// 从注册表检测Git
        /// <para>读取Git for Windows在注册表中的安装记录</para>
        /// </summary>
        /// <returns>Git检测结果对象</returns>
        private static GitDetectResult DetectFromRegistry()
        {
            // 检查64位注册表路径
            using var key64 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\GitForWindows");
            if (key64 != null)
            {
                var installPath = key64.GetValue("InstallPath") as string;
                if (!string.IsNullOrEmpty(installPath))
                {
                    string gitExe = Path.Combine(installPath, "cmd", "git.exe");
                    if (File.Exists(gitExe))
                    {
                        string version = GetGitVersion(gitExe);
                        return new GitDetectResult
                        {
                            IsFound = true,
                            GitPath = gitExe,
                            Version = version,
                            Source = "注册表"
                        };
                    }
                }
            }

            // 检查32位注册表路径（WOW6432Node）
            using var key32 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\GitForWindows");
            if (key32 != null)
            {
                var installPath = key32.GetValue("InstallPath") as string;
                if (!string.IsNullOrEmpty(installPath))
                {
                    string gitExe = Path.Combine(installPath, "cmd", "git.exe");
                    if (File.Exists(gitExe))
                    {
                        string version = GetGitVersion(gitExe);
                        return new GitDetectResult
                        {
                            IsFound = true,
                            GitPath = gitExe,
                            Version = version,
                            Source = "注册表"
                        };
                    }
                }
            }

            return new GitDetectResult { IsFound = false };
        }

        /// <summary>
        /// 获取Git版本信息
        /// </summary>
        /// <param name="gitExePath">Git可执行文件的完整路径</param>
        /// <returns>Git版本字符串，格式为"git version x.y.z.w"，如果获取失败则返回"未知版本"</returns>
        private static string GetGitVersion(string gitExePath)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = gitExePath,
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit(3000);
                return output;
            }
            catch
            {
                return "未知版本";
            }
        }
    }
}