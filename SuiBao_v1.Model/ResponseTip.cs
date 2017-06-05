using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiBao_v1.Model
{
    /// <summary>
    /// 返回到客户端的提示信息
    /// </summary>
    public static class ResponseTip
    {
        public const string ServiceError = "服务异常";

        public const string ParamError = "参数异常";

        public const string ActionSuccess = "请求成功";

        public const string ActionFailed = "请求失败";

        public const string ResultIsNull = "查询结果为空";
    }
}
