using GitLocalService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLocalService.Services
{
    public class JsonProvider
    {
        private static readonly string ConfigDirectory =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs");

        private static readonly string ConfigFilePath =
            Path.Combine(ConfigDirectory, "appsettings.json");

        /// <summary>
        /// 从 Configs/appsettings.json 反序列化读取配置
        /// </summary>
        public static ServiceConfig LoadConfig()
        {
            if (!File.Exists(ConfigFilePath))
            {
                // 文件不存在时返回默认配置
                var defaultConfig = new ServiceConfig();
                SaveConfig(defaultConfig);
                return defaultConfig;
            }

            var json = File.ReadAllText(ConfigFilePath);
            var root = JsonConvert.DeserializeObject<AppSettingsRoot>(json);

            return root?.Default ?? new ServiceConfig();
        }

        /// <summary>
        /// 将配置序列化写入 Configs/appsettings.json
        /// </summary>
        public static void SaveConfig(ServiceConfig config)
        {
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            var root = new AppSettingsRoot { Default = config };
            var json = JsonConvert.SerializeObject(root, Formatting.Indented);
            File.WriteAllText(ConfigFilePath, json);
        }

        #region 内部模型类

        /// <summary>
        /// JSON 根对象，对应 appsettings.json 的最外层结构
        /// </summary>
        internal class AppSettingsRoot
        {
            [JsonProperty("default")]
            public ServiceConfig Default { get; set; } = new ServiceConfig();
        }

        #endregion 内部模型类
    }
}