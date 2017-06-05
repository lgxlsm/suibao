using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SuiBao_v1.Model
{
    public static class OperationContext
    {
        /// <summary>
        /// 从当前线程中获取 线程唯一的 操作上下文
        /// </summary>
        public static wolf_testEntities Current
        {
            get
            {
                wolf_testEntities dbContext = CallContext.LogicalGetData("CurrentContext") as wolf_testEntities;
                if (dbContext == null)
                {
                    dbContext = new wolf_testEntities();
                    dbContext.Configuration.ValidateOnSaveEnabled = false;
                    CallContext.LogicalSetData("CurrentContext", dbContext);
                }
                //dbContext.Database.Log = (s) =>
                //{
                //    System.Diagnostics.Trace.TraceInformation(s);
                //};
                return dbContext;
            }
        }

        /// <summary>
        /// 提交上下文内容，异步方法
        /// </summary>
        public static async Task<int> SaveChangesAsync()
        {
            return await Current.SaveChangesAsync();
        }

        /// <summary>
        /// 提交上下文内容
        /// </summary>
        /// <returns></returns>
        public static int SaveChanges()
        {
            return Current.SaveChanges();
        }
    }
}
