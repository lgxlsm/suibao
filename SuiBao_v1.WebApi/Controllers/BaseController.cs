using System;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using SuiBao_v1.Model;
using SuiBao_v1.Cache;
using System.Linq;


namespace SuiBao_v1.WebApi.Controllers
{
    /// <summary>
    /// api基类
    /// </summary>
    public class BaseController : ApiController
    {
        [Ninject.Inject]
        public ICacheService _cache { get; set; }

        /// <summary>
        /// 接口统一的返回消息
        /// </summary>
        /// <param name="result">返回内容</param>
        /// <param name="cookies">cookie</param>
        /// <returns></returns>
        protected HttpResponseMessage ApiResponse(ReturnMessage result, params CookieHeaderValue[] cookies)
        {
            HttpResponseMessage response = new HttpResponseMessage
            {
                Content = new ObjectContent<ReturnMessage>(result, Configuration.Formatters.JsonFormatter),
            };
            if (cookies.Length > 0)
            {
                response.Headers.AddCookies(cookies);
            }
            CookieHeaderValue token = Request.Headers.GetCookies("token").FirstOrDefault();
            if (token != null)
            {
                var cookieToken = new CookieHeaderValue("token", token["token"].Value)
                {
                    Expires = DateTimeOffset.Now.AddHours(1),
                    Domain = Request.RequestUri.Host,
                    Path = "/",
                    HttpOnly = true
                };
                response.Headers.AddCookies(new[] { cookieToken });
            }
            return response;
        }

        /// <summary>
        /// 当前请求会员的身份token
        /// </summary>
        protected string CurrentUserToken
        {
            get
            {
                string cookieName = "token";
                CookieHeaderValue cookie = Request.Headers.GetCookies(cookieName).FirstOrDefault();
                if (cookie != null)
                {
                    return cookie[cookieName].Value;
                }
                return null;
            }
        }

        /// <summary>
        /// 当前请求的会员id
        /// </summary>
        protected string CurrentUserId
        {
            get { return System.Web.Security.FormsAuthentication.Decrypt(CurrentUserToken).UserData; }
        }

        /// <summary>
        /// 当前请求的用户信息
        /// </summary>
        protected People CurrentUserInfo
        {
            get
            {
                return _cache.Get<People>(CurrentUserToken);
            }
        }


    }
}