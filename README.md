# wuwh.DdnsSvr

项目地址：https://github.com/hnwywwh/wuwh.DdnsSvr

已生成docker镜像：https://registry.hub.docker.com/33747243/wuwhddnssvr

功能：更新alidns解析记录，启动docker容器，映射80端口出来，即可使用URL进行更新

以ROS为例：
/system scripts
添加脚本：

#ddns更新接口地址，是否延时根据实际情况定义
#:delay 10s 
:local url "http://192.168.1.100:20088"

:local AccessKeyID "xxxxxxxxxx"

:local AccessKeySecret "xxxxxxxxxxxxx"

#需要更新的域名,域名必须在阿里云上,如果是根域名请指定@  ，如china.cn的根域名为：@.china.com
:local name "*.china.cn"

#更新域名IP的接口
:local Interface "pppoe-out1"

#########以下脚本代码, 不懂ros脚本,请不要随意修改#######################
:local localip

:foreach i in=[/ip address find interface=$Interface ] do={

:set localip [/ip address get $i address ]}

:set localip [:pick $localip 0 [find $localip /]]

/tool fetch url=($url ."/api/AliDns/UpdateIp/\?&domain=$name&accessKeyId=$AccessKeyID&secret=$AccessKeySecret&ip=$localip" ) keep-result=no

:log info "aliyun ddns更新$Interface成功:ip=$localip"
