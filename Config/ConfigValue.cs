using Aliyun.Acs.Core.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wuwh.DdnsSvr.Config
{
    public class ConfigValue
    {
        /// <summary>
        /// 默认地域
        /// </summary>
        public static string RegionId { get; set; } = "cn-shenzhen";
        /// <summary>
        /// 获取客户端凭证
        /// </summary>
        /// <param name="regionId">地域ID</param>
        /// <param name="accessKeyId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static IClientProfile GetClientProfile(string regionId, string accessKeyId, string secret)
        {
            return DefaultProfile.GetProfile(regionId, accessKeyId, secret);
        }
        public static IClientProfile GetClientProfile( string accessKeyId, string secret)
        {
            return DefaultProfile.GetProfile(RegionId, accessKeyId, secret);
        }
    }
}
