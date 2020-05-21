using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.Acs.Alidns.Model.V20150109;
using Aliyun.Acs.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wuwh.DdnsSvr.Config;

namespace wuwh.DdnsSvr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AliDnsController : ControllerBase
    {
        [HttpGet("UpdateIp")]
        public async Task<IActionResult> UpdateIp(string domain,string accessKeyId, string secret,string ip)
        {
            try
            {
                var index = domain.IndexOf('.');
                if (index < 1) return BadRequest("域名格式错误");
                var subDomainName = domain.Substring(0, index);
                var domainName = domain.Substring(index + 1, domain.Length - index - 1);
                DefaultAcsClient client = new DefaultAcsClient(ConfigValue.GetClientProfile(accessKeyId, secret));
                DescribeDomainRecordsRequest request = new DescribeDomainRecordsRequest();
                request.DomainName = domainName;
                DescribeDomainRecordsResponse response = await Task.Run(() => client.GetAcsResponse(request));
                if (response.TotalCount > 0)
                {
                    var subDomainRecordList = response.DomainRecords.Where(t => t.RR == subDomainName);
                    if (subDomainRecordList.Count() < 1) return BadRequest("子域名不存在");
                    var subDomainRecord = subDomainRecordList.First();
                    if (subDomainRecord._Value == ip) return new JsonResult(new { status = "success", msg = "IP未发生变动，无需更新" });
                    UpdateDomainRecordRequest upRequest = new UpdateDomainRecordRequest();
                    upRequest.RR = subDomainName;
                    upRequest.RecordId = subDomainRecord.RecordId;
                    upRequest.Type = "A";
                    upRequest._Value = ip;
                    UpdateDomainRecordResponse upRes = await Task.Run(() => client.GetAcsResponse(upRequest));

                    return new JsonResult(new { status = "success", msg = $"IP更新成功，ID为：{upRes.RequestId}" });
                }
                return new JsonResult(new { status = "fail", msg = "未获取到域名解析列表" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = "fail", msg = ex.Message});
            }
        }
    }
}