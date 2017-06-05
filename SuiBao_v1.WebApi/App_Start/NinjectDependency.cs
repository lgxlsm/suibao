using SuiBao_v1.Ioc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Web.Http.Dependencies;
namespace SuiBao_v1.WebApi.App_Start
{
    public class DependencyResolverForWebApi : NinjectDependencyScope, IDependencyResolver
    {
        private readonly IContainer kernel;

        public DependencyResolverForWebApi(IContainer kernel) : base(kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            this.kernel = kernel;
        }


        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel);
        }
    }

    public class NinjectDependencyScope : IDependencyScope
    {
        private IContainer resolver;

        internal NinjectDependencyScope(IContainer resolver)
        {
            Contract.Assert(resolver != null);
            this.resolver = resolver;
        }
        public void Dispose()
        {
            resolver = null;
        }

        public object GetService(Type serviceType)
        {
            return resolver.Get(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return resolver.GetAll(serviceType);
        }
    }
}