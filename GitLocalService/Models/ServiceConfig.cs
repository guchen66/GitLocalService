using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLocalService.Models
{
    /// <summary>
    /// 服务配置类，对应 JSON 中 "default" 节点
    /// </summary>
    public class ServiceConfig
    {
        private bool _acceptLicense;

        [JsonProperty("AcceptLicense")]
        public bool AcceptLicense
        {
            get => _acceptLicense;
            set
            {
                _acceptLicense = value;
                // 自动同步 NotAcceptLicense，保持两者互斥
                _notAcceptLicense = !value;
            }
        }

        private bool _notAcceptLicense = true;

        /// <summary>
        /// 不写入 JSON，由 AcceptLicense 取反自动计算
        /// </summary>
        [JsonIgnore]
        public bool NotAcceptLicense
        {
            get => _notAcceptLicense;
            set
            {
                _notAcceptLicense = value;
                // 反向同步，确保一致性
                _acceptLicense = !value;
            }
        }

        [JsonProperty("RegistryRightKey")]
        public bool RegistryRightKey { get; set; }
    }
}