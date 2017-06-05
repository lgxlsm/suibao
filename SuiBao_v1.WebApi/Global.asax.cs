using SuiBao_v1.Cache;
using SuiBao_v1.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SuiBao_v1.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private ICacheService _cache { get; set; }
        protected void Application_Start()
        {
            //加载log4配置
            log4net.Config.XmlConfigurator.Configure();

            //注册Ninject 
            App_Start.NinjectRegister.RegisterForWebApi(GlobalConfiguration.Configuration);

            //2.启动缓存服务
            _cache = new RedisProvider();
            _cache.StartUp();

            //EF监控
            DbInterception.Add(new EFIntercepterLogging());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
