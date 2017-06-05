namespace SuiBao_v1.Ioc
{
    public class ContainerHost : IContainerHost
    {
        private static readonly NinjectContainer _container = new NinjectContainer();

        public IContainer GetContainer()
        {
            //可以根据参数获取需要的ioc容器
            return _container;
        }
    }
}
