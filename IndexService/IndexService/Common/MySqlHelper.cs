using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace IndexService.Common
{
    public class MySqlHelper
    {
        private static readonly string mysqlcon = ConfigurationManager.ConnectionStrings["mysqlcon"].ConnectionString;

        public static DataTable Search(string sql,params MySqlParameter[] parmters)
        {
            using (MySqlConnection conn = new MySqlConnection(mysqlcon))
            {
                MysqlOpen(conn);
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parmters);
                    DataSet ds = new DataSet();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    MysqlClosed(conn);
                    return ds.Tables[0];
                }
            }
        }

        private static void MysqlOpen(MySqlConnection _conn)
        {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }
        }

        private static void MysqlClosed(MySqlConnection _conn)
        {
            if (_conn.State != ConnectionState.Closed)
            {
                _conn.Close();
            }
        }
    }
}
