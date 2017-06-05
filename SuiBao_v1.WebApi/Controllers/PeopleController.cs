using SuiBao_v1.Model;
using SuiBao_v1.WebApi.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SuiBao_v1.WebApi.Controllers
{
    /// <summary>
    /// 做个例子
    /// </summary>
    public class PeopleController : BaseController
    {
        [Ninject.Inject]
        private IBusiness.IWolfTest _wolfTest { get; set; }

        /// <summary>
        /// 模拟登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost, Route("api/Account/PasswordLogin")]
        public HttpResponseMessage Login()
        {
            string authToken = Guid.NewGuid().ToString();
            People model = new People()
            {
                Name = "7777",
                Remark = "88888"
            };
            _cache.Set(authToken, model, 60 * 60);
            CookieHeaderValue[] cookies = new CookieHeaderValue[2];
            //写入客户端cookie
            cookies[0] = new CookieHeaderValue("token", authToken)
            {
                Expires = DateTimeOffset.Now.AddHours(1),
                Domain = Request.RequestUri.Host,
                Path = "/",
                HttpOnly = true
            };
            cookies[1] = new CookieHeaderValue("piwik", authToken)
            {
                Domain = Request.RequestUri.Host,
                Path = "/"
            };
            HttpResponseMessage response = new HttpResponseMessage
            {
                Content = new ObjectContent<People>(model, Configuration.Formatters.JsonFormatter),
            };
            response.Headers.AddCookies(cookies);
            return response;
        }

        /// <summary>
        /// 注销登陆
        /// </summary>
        /// <returns></returns>
        [Authorization, HttpPost, Route("api/Account/LoginOut")]
        public HttpResponseMessage LoginOut()
        {
            _cache.RemoveKey(CurrentUserToken);
            //删除客户端cookie
            var cookie = new CookieHeaderValue("token", string.Empty)
            {
                Expires = DateTimeOffset.Now.AddDays(-1),
                Domain = Request.RequestUri.Host,
                Path = "/",
                HttpOnly = true
            };
            var piwik = new CookieHeaderValue("piwik", string.Empty)
            {
                Expires = DateTimeOffset.Now.AddDays(-1),
                Domain = Request.RequestUri.Host,
                Path = "/"
            };
            return ApiResponse(new ReturnMessage() { Status = ResultStatus.Success, Message = "注销登陆" }, cookie, piwik);
        }

        /// <summary>
        /// 测试操作数据库-插入数据
        /// </summary>
        /// <param name="agrs">随便给个参数</param>
        /// <returns></returns>
        [Authorization, HttpPost, Route("api/Account/Test1")]
        public HttpResponseMessage Test(string agrs)
        {
            string aa = CurrentUserInfo.Name;
            var result = _wolfTest.Test1();
            return ApiResponse(result);
        }

        /// <summary>
        /// 测试操作数据库-查询数据库
        /// </summary>
        /// <param name="agrs">随便给个参数</param>
        /// <returns></returns>
        [Authorization, HttpPost, Route("api/Account/Test2")]
        public HttpResponseMessage Test(int agrs)
        {
            string aa = CurrentUserInfo.Name;
            var result = _wolfTest.Test2();
            return ApiResponse(result);
        }
    }
}
