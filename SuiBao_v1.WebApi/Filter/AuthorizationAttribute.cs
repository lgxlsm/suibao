using SuiBao_v1.Cache;
using SuiBao_v1.Model;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net.Http.Headers;


namespace SuiBao_v1.WebApi.Filter
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = true)]
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        [Ninject.Inject]
        public ICacheService _cache { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // 匿名访问验证
            var anonymousAction = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>();
            if (!anonymousAction.Any())
            {
                ReturnMessage result = null;
                // token存在验证
                CookieHeaderValue cookie = actionContext.ControllerContext.Request.Headers.GetCookies("token").FirstOrDefault();
                if (cookie == null)
                {
                    result = new ReturnMessage(ResultStatus.UnAuthorize, "登陆失败");
                }
                else
                {
                    //token合法验证
                    var user = _cache.Get(cookie["token"].Value);
                    if (string.IsNullOrEmpty(user))
                    {
                        result = new ReturnMessage(ResultStatus.UnAuthorize, "登陆失败");
                    }
                }
                if (result != null)
                {
                    var response = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
                    response.Content = new ObjectContent<ReturnMessage>(result,
                        actionContext.ActionDescriptor.Configuration.Formatters.JsonFormatter);
                    actionContext.Response = response;
                    return;
                }
                _cache.KeyExpire(cookie["token"].Value, 60 * 60);
            }
        }
    }
}