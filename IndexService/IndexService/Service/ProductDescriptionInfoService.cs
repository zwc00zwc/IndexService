using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexService.Interface;
using IndexService.Model;
using System.Data;
using MySql.Data.MySqlClient;

namespace IndexService.Service
{
    public class ProductDescriptionInfoService:IProductDescriptionInfo
    {
        public string GetByProductId(long id)
        {
            string info = "";
            DataTable dt = new DataTable();
            MySqlParameter pidpartmter = new MySqlParameter("pid", MySqlDbType.Int64);
            pidpartmter.Value = id;
            dt = IndexService.Common.MySqlHelper.Search("SELECT Description FROM Himall_ProductDescriptions WHERE ProductId=@pid", new MySqlParameter[] { pidpartmter });
            if (dt.Rows.Count > 0)
            {
                info = dt.Rows[0]["Description"].ToString();
            }
            return info;
        }
    }
}
