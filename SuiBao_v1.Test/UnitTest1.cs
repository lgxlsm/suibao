using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuiBao_v1.Business;
using Ninject;
using SuiBao_v1.IRepository;
using SuiBao_v1.Repository;
using SuiBao_v1.Cache;
using SuiBao_v1.IBusiness;

namespace SuiBao_v1.Test
{
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void TestMethod1()
        {
            //实例化Ninject对象
            IKernel Kerner = new StandardKernel();
            Kerner.Bind<IRepositoryFactory>().To<RepositoryFactory>();
            Kerner.Bind<ICacheService>().To<RedisProvider>();
            Kerner.Bind<IWolfTest>().To<WolfTest>();
            //允许私有属性注入
            Kerner.Settings.InjectNonPublic = true;
            Kerner.Settings.InjectParentPrivateProperties = true;
            var _wolfTest = Kerner.Get<IWolfTest>();
            _wolfTest.Test1();
            _wolfTest.Test2();
        }
    }

}
