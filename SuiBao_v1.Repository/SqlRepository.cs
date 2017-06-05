using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiBao_v1.Repository
{
    public class SqlRepository : SuiBao_v1.IRepository.ISqlRepository
    {
        /// <summary>
        /// EF 容器对象
        /// </summary>
        private readonly DbContext db = SuiBao_v1.Model.OperationContext.Current;

        /// <summary>
        /// 直接执行 sql语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public int ExcuteNonQuery(string strSql, params System.Data.SqlClient.SqlParameter[] paras)
        {
            return db.Database.ExecuteSqlCommand(strSql, paras);
        }

        /// <summary>
        /// SQL语句查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public List<T> SqlQuery<T>(string Sql)
        {
            return db.Database.SqlQuery<T>(Sql).ToList();
        }
    }
}
