using System;
using System.Collections.Generic;

namespace SuiBao_v1.Ioc
{
    public interface IContainerHost
    {
        IContainer GetContainer();
    }

    /// <summary>
    /// IOC容器的公共接口
    /// </summary>
    public interface IContainer
    {
        object GetKernel();

        T Get<T>();

        object Get(Type t);

        IEnumerable<object> GetAll(Type t);

        IEnumerable<T> GetAll<T>();

        void Register<T, IT>() where IT : T;

        void Remove<T>();

        void Inject(object obj);
    }
}
