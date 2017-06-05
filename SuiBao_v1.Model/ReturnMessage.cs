using System.ComponentModel;

namespace SuiBao_v1.Model
{
    public class ReturnMessage
    {
        public ReturnMessage() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="status"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public ReturnMessage(ResultStatus status, string msg = "", object data = null, string url = "")
        {
            this.Status = status;
            this.Message = msg;
            this.Data = data;
            this.Url = url;
        }

        /// <summary>
        /// 返回状态码
        /// </summary>
        public ResultStatus Status { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 重定向地址
        /// </summary>
        public string Url { get; set; }
    }

    public enum ResultStatus
    {
        /// <summary>
        /// 服务异常
        /// </summary>
        [Description("服务异常")]
        ServiceError = -2,

        /// <summary>
        /// 非法请求
        /// </summary>
        [Description("非法请求")]
        Illegal = -1,

        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("请求成功")]
        Success = 0,

        /// <summary>
        /// 请求无效
        /// </summary>
        [Description("请求无效")]
        Disable = 1,

        /// <summary>
        /// 登录失败
        /// </summary>
        [Description("登录失败")]
        UnAuthorize = 2,

        /// <summary>
        /// 参数异常
        /// </summary>
        [Description("参数异常")]
        ParamError = 3,

        /// <summary>
        /// 数据异常
        /// </summary>
        [Description("数据异常")]
        DataException = 4,

        /// <summary>
        /// 验证失败
        /// </summary>
        [Description("验证失败")]
        ValidateError = 5
    }
}
