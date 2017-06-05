using Ninject;
using SuiBao_v1.Business;
using SuiBao_v1.Cache;
using SuiBao_v1.IBusiness;
using SuiBao_v1.Ioc;
using SuiBao_v1.IRepository;
using SuiBao_v1.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SuiBao_v1.WebApi.App_Start
{
    public static class NinjectRegister
    {
        /// <summary>
        /// WebApi项目的注册
        /// </summary>
        /// <param name="config"></param>
        public static void RegisterForWebApi(HttpConfiguration config)
        {
            IContainer _ninjectContainer = new ContainerHost().GetContainer();

            //为ActionFilter注册服务
            var providers = config.Services.GetFilterProviders().ToList();
            var defaultprovider = providers.Single(i => i is ActionDescriptorFilterProvider);
            config.Services.Remove(typeof(IFilterProvider), defaultprovider);
            config.Services.Add(typeof(IFilterProvider), new WebApiActionFilterProvider(_ninjectContainer));

            //binding commom service
            _ninjectContainer.Register<IRepositoryFactory, RepositoryFactory>();
            _ninjectContainer.Register<ICacheService, RedisProvider>();


            //binding business
            _ninjectContainer.Register<IWolfTest, WolfTest>();

            //注入到全局配置
            config.DependencyResolver = new DependencyResolverForWebApi(_ninjectContainer);
        }
    }

    public class WebApiActionFilterProvider : ActionDescriptorFilterProvider, IFilterProvider
    {
        private readonly IContainer _container;

        public WebApiActionFilterProvider(IContainer container)
        {
            this._container = container;
        }

        public new IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(configuration, actionDescriptor);
            foreach (var filter in filters)
            {
                _container.Inject(filter.Instance);
            }
            return filters;
        }
    }
}