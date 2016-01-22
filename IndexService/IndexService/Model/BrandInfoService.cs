using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Model;
using IndexService.Common;
using IndexService.Interface;
using System.Data;
using MySql.Data.MySqlClient;

namespace IndexService.Model
{
    public class BrandInfoService:IBrandInfo
    {
        public string GetNameById(long id)
        {
            string info = "";
            DataTable dt = new DataTable();
            MySqlParameter idparamter=new MySqlParameter("bid",MySqlDbType.Int64);
            idparamter.Value = id;
            dt = IndexService.Common.MySqlHelper.Search("SELECT Name FROM Himall_Brands WHERE Id=@bid", new MySqlParameter[] { idparamter });
            if (dt.Rows.Count > 0)
            {
                info = dt.Rows[0]["Name"].ToString();
            }
            return info;
        }
    }
}
