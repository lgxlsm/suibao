using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiBao_v1.IRepository
{
    public interface ISqlRepository
    {
        /// <summary>
        /// 直接执行 sql语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        int ExcuteNonQuery(string strSql, params System.Data.SqlClient.SqlParameter[] paras);

        List<T> SqlQuery<T>(string Sql);
    }
}
