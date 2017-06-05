using Ninject;
using System;
using System.Collections.Generic;

namespace SuiBao_v1.Ioc
{
    public class NinjectContainer : IContainer
    {
        private IKernel _kernel = null;

        /// <summary>
        /// 构造函数创建kernel实例
        /// </summary>
        public NinjectContainer()
        {
            if (_kernel == null)
            {
                _kernel = new StandardKernel();
            }
            //允许私有属性注入
            _kernel.Settings.InjectNonPublic = true;
            _kernel.Settings.InjectParentPrivateProperties = true;
        }

        public object GetKernel()
        {
            return _kernel;
        }

        public object Get(Type t)
        {
            return _kernel.TryGet(t);
        }

        public IEnumerable<object> GetAll(Type t)
        {
            return _kernel.GetAll(t);
        }

        public T Get<T>()
        {
            return _kernel.TryGet<T>();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _kernel.GetAll<T>();
        }

        public void Register<T, IT>() where IT : T
        {
            _kernel.Bind<T>().To<IT>();
        }

        public void Inject(object obj)
        {
            _kernel.Inject(obj);
        }

        public void Remove<T>()
        {
            throw new NotImplementedException();
        }
    }
}
