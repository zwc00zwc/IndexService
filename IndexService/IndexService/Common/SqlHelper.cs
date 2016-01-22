using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace IndexService.Common
{
    public class SqlHelper
    {
        private static readonly string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public static DataTable Search(string sql, params SqlParameter[] parmters)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                sqlOpen(con);
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parmters);
                    cmd.CommandTimeout = 500;
                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    sqlClose(con);
                    return ds.Tables[0];
                }
            }
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="con"></param>
        private static void sqlOpen(SqlConnection con)
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="con"></param>
        private static void sqlClose(SqlConnection con)
        {
            if (con.State != ConnectionState.Closed)
            {
                con.Close();
            }
        }
    }
}
